using System.CodeDom.Compiler;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Net.Http.Headers;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text;
using Newtonsoft.Json;

namespace Instagram.Concrete;

#nullable disable

internal class LamavadaClient : ILamavadaClient
{
	private readonly HttpClient _httpClient;
	private readonly Lazy<JsonSerializerSettings> _settings;

	public LamavadaClient(IHttpClientFactory httpClientFactory)
	{
		BaseUrl = "";
		_httpClient = httpClientFactory.CreateClient();
		_settings = new Lazy<JsonSerializerSettings>(CreateSerializerSettings);
	}

	private JsonSerializerSettings CreateSerializerSettings()
	{
		var settings = new JsonSerializerSettings();
		UpdateJsonSerializerSettings(settings);
		return settings;
	}

	private string BaseUrl { get; set; } = "";

	protected JsonSerializerSettings JsonSerializerSettings => _settings.Value;

	void UpdateJsonSerializerSettings(JsonSerializerSettings settings) { }

	void PrepareRequest(HttpClient client, HttpRequestMessage request, string url) { }
	void PrepareRequest(HttpClient client, HttpRequestMessage request, StringBuilder urlBuilder) { }
	void ProcessResponse(HttpClient client, HttpResponseMessage response) { }

	protected struct ObjectResponseResult<T>
	{
		public ObjectResponseResult(T responseObject, string responseText)
		{
			Object = responseObject;
			Text = responseText;
		}

		public T Object { get; }

		public string Text { get; }
	}
	public bool ReadResponseAsString { get; set; }
	protected virtual async Task<ObjectResponseResult<T>> ReadObjectResponseAsync<T>(HttpResponseMessage response, IReadOnlyDictionary<string, IEnumerable<string>> headers, CancellationToken cancellationToken)
	{
		if (response == null || response.Content == null)
		{
			return new ObjectResponseResult<T>(default(T), string.Empty);
		}

		if (ReadResponseAsString)
		{
			var responseText = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
			try
			{
				var typedBody = JsonConvert.DeserializeObject<T>(responseText, JsonSerializerSettings);
				return new ObjectResponseResult<T>(typedBody, responseText);
			}
			catch (JsonException exception)
			{
				var message = "Could not deserialize the response body string as " + typeof(T).FullName + ".";
				throw new ApiException(message, (int)response.StatusCode, responseText, headers, exception);
			}
		}

		try
		{
			using var responseStream = await response.Content.ReadAsStreamAsync().ConfigureAwait(false);
			using var streamReader = new StreamReader(responseStream);
			using var jsonTextReader = new JsonTextReader(streamReader);
			var serializer = JsonSerializer.Create(JsonSerializerSettings);
			var typedBody = serializer.Deserialize<T>(jsonTextReader);
			return new ObjectResponseResult<T>(typedBody, string.Empty);
		}
		catch (JsonException exception)
		{
			var message = "Could not deserialize the response body stream as " + typeof(T).FullName + ".";
			throw new ApiException(message, (int)response.StatusCode, string.Empty, headers, exception);
		}
	}
	private string ConvertToString(object value, CultureInfo cultureInfo)
	{
		if (value == null)
		{
			return "";
		}

		if (value is Enum)
		{
			var name = Enum.GetName(value.GetType(), value);
			if (name != null)
			{
				var field = value.GetType().GetTypeInfo().GetDeclaredField(name);
				if (field != null)
				{
					var attribute = field.GetCustomAttribute(typeof(EnumMemberAttribute))
						as EnumMemberAttribute;
					if (attribute != null)
					{
						return attribute.Value != null ? attribute.Value : name;
					}
				}

				var converted = Convert.ToString(Convert.ChangeType(value, Enum.GetUnderlyingType(value.GetType()), cultureInfo));
				return converted == null ? string.Empty : converted;
			}
		}
		else if (value is bool)
		{
			return Convert.ToString((bool)value, cultureInfo).ToLowerInvariant();
		}
		else if (value is byte[])
		{
			return Convert.ToBase64String((byte[])value);
		}
		else if (value.GetType().IsArray)
		{
			var array = ((Array)value).OfType<object>();
			return string.Join(",", array.Select(o => ConvertToString(o, cultureInfo)));
		}

		var result = Convert.ToString(value, cultureInfo);
		return result == null ? "" : result;
	}


	public async Task<User> GetUserById(string id, CancellationToken cancellationToken)
	{
		if (id == null)
			throw new ArgumentNullException(nameof(id));

		var urlBuilder = new StringBuilder();
		urlBuilder.Append(BaseUrl != null ? BaseUrl.TrimEnd('/') : "").Append("/v1/user/by/id?");
		urlBuilder.Append(Uri.EscapeDataString("id") + "=").Append(Uri.EscapeDataString(ConvertToString(id, CultureInfo.InvariantCulture))).Append("&");
		urlBuilder.Length--;

		var client = _httpClient;
		var disposeClient = false;
		try
		{
			using var request = new HttpRequestMessage();
			request.Method = new HttpMethod("GET");
			request.Headers.Accept.Add(MediaTypeWithQualityHeaderValue.Parse("application/json"));

			PrepareRequest(client, request, urlBuilder);

			var url = urlBuilder.ToString();
			request.RequestUri = new Uri(url, UriKind.RelativeOrAbsolute);

			PrepareRequest(client, request, url);

			var response = await client.SendAsync(request, HttpCompletionOption.ResponseHeadersRead, cancellationToken).ConfigureAwait(false);
			var disposeResponse = true;
			try
			{
				var headers = response.Headers.ToDictionary(h => h.Key, h => h.Value);
				if (response.Content != null && response.Content.Headers != null)
				{
					foreach (var item in response.Content.Headers)
						headers[item.Key] = item.Value;
				}

				ProcessResponse(client, response);

				var status = (int)response.StatusCode;
				if (status == 200)
				{
					var objectResponse = await ReadObjectResponseAsync<User>(response, headers, cancellationToken).ConfigureAwait(false);
					if (objectResponse.Object == null)
					{
						throw new ApiException("Response was null which was not expected.", status, objectResponse.Text, headers, null);
					}
					return objectResponse.Object;
				}
				else
				if (status == 404)
				{
					string responseText = (response.Content == null) ? string.Empty : await response.Content.ReadAsStringAsync().ConfigureAwait(false);
					throw new ApiException("Not found", status, responseText, headers, null);
				}
				else
				if (status == 422)
				{
					var objectResponse = await ReadObjectResponseAsync<HttpValidationError>(response, headers, cancellationToken).ConfigureAwait(false);
					if (objectResponse.Object == null)
					{
						throw new ApiException("Response was null which was not expected.", status, objectResponse.Text, headers, null);
					}
					throw new ApiException<HttpValidationError>("Validation Error", status, objectResponse.Text, headers, objectResponse.Object, null);
				}
				else
				{
					var responseData = response.Content == null ? null : await response.Content.ReadAsStringAsync().ConfigureAwait(false);
					throw new ApiException("The HTTP status code of the response was not expected (" + status + ").", status, responseData, headers, null);
				}
			}
			finally
			{
				if (disposeResponse)
					response.Dispose();
			}
		}
		finally
		{
			if (disposeClient)
				client.Dispose();
		}
	}
	public async Task<User> GetUserByUsername(string username, CancellationToken cancellationToken)
	{
		if (username == null)
			throw new ArgumentNullException(nameof(username));

		var urlBuilder = new StringBuilder();
		urlBuilder.Append(BaseUrl != null ? BaseUrl.TrimEnd('/') : "").Append("/v1/user/by/username?");
		urlBuilder.Append(Uri.EscapeDataString("username") + "=").Append(Uri.EscapeDataString(ConvertToString(username, CultureInfo.InvariantCulture))).Append("&");
		urlBuilder.Length--;

		var client = _httpClient;
		var disposeClient = false;
		try
		{
			using var request = new HttpRequestMessage();
			request.Method = new HttpMethod("GET");
			request.Headers.Accept.Add(MediaTypeWithQualityHeaderValue.Parse("application/json"));

			PrepareRequest(client, request, urlBuilder);

			var url = urlBuilder.ToString();
			request.RequestUri = new Uri(url, UriKind.RelativeOrAbsolute);

			PrepareRequest(client, request, url);

			var response = await client.SendAsync(request, HttpCompletionOption.ResponseHeadersRead, cancellationToken).ConfigureAwait(false);
			var disposeResponse = true;
			try
			{
				var headers = response.Headers.ToDictionary(h => h.Key, h => h.Value);
				if (response.Content != null && response.Content.Headers != null)
				{
					foreach (var item in response.Content.Headers)
						headers[item.Key] = item.Value;
				}

				ProcessResponse(client, response);

				var status = (int)response.StatusCode;
				if (status == 200)
				{
					var objectResponse = await ReadObjectResponseAsync<User>(response, headers, cancellationToken).ConfigureAwait(false);
					if (objectResponse.Object == null)
					{
						throw new ApiException("Response was null which was not expected.", status, objectResponse.Text, headers, null);
					}
					return objectResponse.Object;
				}
				else
				if (status == 404)
				{
					string responseText = (response.Content == null) ? string.Empty : await response.Content.ReadAsStringAsync().ConfigureAwait(false);
					throw new ApiException("Not found", status, responseText, headers, null);
				}
				else
				if (status == 422)
				{
					var objectResponse = await ReadObjectResponseAsync<HttpValidationError>(response, headers, cancellationToken).ConfigureAwait(false);
					if (objectResponse.Object == null)
					{
						throw new ApiException("Response was null which was not expected.", status, objectResponse.Text, headers, null);
					}
					throw new ApiException<HttpValidationError>("Validation Error", status, objectResponse.Text, headers, objectResponse.Object, null);
				}
				else
				{
					var responseData = response.Content == null ? null : await response.Content.ReadAsStringAsync().ConfigureAwait(false);
					throw new ApiException("The HTTP status code of the response was not expected (" + status + ").", status, responseData, headers, null);
				}
			}
			finally
			{
				if (disposeResponse)
					response.Dispose();
			}
		}
		finally
		{
			if (disposeClient)
				client.Dispose();
		}
	}

    public async Task<ICollection<UserShort>> SearchFollowers(string userId, string query, CancellationToken cancellationToken)
	{
		if (userId == null)
			throw new ArgumentNullException(nameof(userId));

		if (query == null)
			throw new ArgumentNullException(nameof(query));

		var urlBuilder = new StringBuilder();
		urlBuilder.Append(BaseUrl != null ? BaseUrl.TrimEnd('/') : "").Append("/v1/user/search/followers?");
		urlBuilder.Append(Uri.EscapeDataString("user_id") + "=").Append(Uri.EscapeDataString(ConvertToString(userId, CultureInfo.InvariantCulture))).Append("&");
		urlBuilder.Append(Uri.EscapeDataString("query") + "=").Append(Uri.EscapeDataString(ConvertToString(query, CultureInfo.InvariantCulture))).Append("&");
		urlBuilder.Length--;

		var client = _httpClient;
		var disposeClient = false;
		try
		{
			using var request = new HttpRequestMessage();
			request.Method = new HttpMethod("GET");
			request.Headers.Accept.Add(MediaTypeWithQualityHeaderValue.Parse("application/json"));

			PrepareRequest(client, request, urlBuilder);

			var url = urlBuilder.ToString();
			request.RequestUri = new Uri(url, UriKind.RelativeOrAbsolute);

			PrepareRequest(client, request, url);

			var response = await client.SendAsync(request, HttpCompletionOption.ResponseHeadersRead, cancellationToken).ConfigureAwait(false);
			var disposeResponse = true;
			try
			{
				var headers = response.Headers.ToDictionary(h => h.Key, h => h.Value);
				if (response.Content != null && response.Content.Headers != null)
				{
					foreach (var item in response.Content.Headers)
						headers[item.Key] = item.Value;
				}

				ProcessResponse(client, response);

				var status = (int)response.StatusCode;
				if (status == 200)
				{
					var objectResponse = await ReadObjectResponseAsync<ICollection<UserShort>>(response, headers, cancellationToken).ConfigureAwait(false);
					if (objectResponse.Object == null)
					{
						throw new ApiException("Response was null which was not expected.", status, objectResponse.Text, headers, null);
					}
					return objectResponse.Object;
				}
				else
				if (status == 404)
				{
					string responseText = (response.Content == null) ? string.Empty : await response.Content.ReadAsStringAsync().ConfigureAwait(false);
					throw new ApiException("Not found", status, responseText, headers, null);
				}
				else
				if (status == 422)
				{
					var objectResponse = await ReadObjectResponseAsync<HttpValidationError>(response, headers, cancellationToken).ConfigureAwait(false);
					if (objectResponse.Object == null)
					{
						throw new ApiException("Response was null which was not expected.", status, objectResponse.Text, headers, null);
					}
					throw new ApiException<HttpValidationError>("Validation Error", status, objectResponse.Text, headers, objectResponse.Object, null);
				}
				else
				{
					var responseData = response.Content == null ? null : await response.Content.ReadAsStringAsync().ConfigureAwait(false);
					throw new ApiException("The HTTP status code of the response was not expected (" + status + ").", status, responseData, headers, null);
				}
			}
			finally
			{
				if (disposeResponse)
					response.Dispose();
			}
		}
		finally
		{
			if (disposeClient)
				client.Dispose();
		}
	}

    public async Task<ICollection<UserShort>> GetFollowers(string userId, int? amount, CancellationToken cancellationToken)
	{
		if (userId == null)
			throw new ArgumentNullException(nameof(userId));

		var urlBuilder = new StringBuilder();
		urlBuilder.Append(BaseUrl != null ? BaseUrl.TrimEnd('/') : "").Append("/v1/user/followers?");
		urlBuilder.Append(Uri.EscapeDataString("user_id") + "=").Append(Uri.EscapeDataString(ConvertToString(userId, CultureInfo.InvariantCulture))).Append("&");
		if (amount != null)
		{
			urlBuilder.Append(Uri.EscapeDataString("amount") + "=").Append(Uri.EscapeDataString(ConvertToString(amount, CultureInfo.InvariantCulture))).Append("&");
		}
		urlBuilder.Length--;

		var client = _httpClient;
		var disposeClient = false;
		try
		{
			using var request = new HttpRequestMessage();
			request.Method = new HttpMethod("GET");
			request.Headers.Accept.Add(MediaTypeWithQualityHeaderValue.Parse("application/json"));

			PrepareRequest(client, request, urlBuilder);

			var url = urlBuilder.ToString();
			request.RequestUri = new Uri(url, UriKind.RelativeOrAbsolute);

			PrepareRequest(client, request, url);

			var response = await client.SendAsync(request, HttpCompletionOption.ResponseHeadersRead, cancellationToken).ConfigureAwait(false);
			var disposeResponse = true;
			try
			{
				var headers = response.Headers.ToDictionary(h => h.Key, h => h.Value);
				if (response.Content != null && response.Content.Headers != null)
				{
					foreach (var item in response.Content.Headers)
						headers[item.Key] = item.Value;
				}

				ProcessResponse(client, response);

				var status = (int)response.StatusCode;
				if (status == 200)
				{
					var objectResponse = await ReadObjectResponseAsync<ICollection<UserShort>>(response, headers, cancellationToken).ConfigureAwait(false);
					if (objectResponse.Object == null)
					{
						throw new ApiException("Response was null which was not expected.", status, objectResponse.Text, headers, null);
					}
					return objectResponse.Object;
				}
				else
				if (status == 404)
				{
					string responseText = (response.Content == null) ? string.Empty : await response.Content.ReadAsStringAsync().ConfigureAwait(false);
					throw new ApiException("Not found", status, responseText, headers, null);
				}
				else
				if (status == 422)
				{
					var objectResponse = await ReadObjectResponseAsync<HttpValidationError>(response, headers, cancellationToken).ConfigureAwait(false);
					if (objectResponse.Object == null)
					{
						throw new ApiException("Response was null which was not expected.", status, objectResponse.Text, headers, null);
					}
					throw new ApiException<HttpValidationError>("Validation Error", status, objectResponse.Text, headers, objectResponse.Object, null);
				}
				else
				{
					var responseData = response.Content == null ? null : await response.Content.ReadAsStringAsync().ConfigureAwait(false);
					throw new ApiException("The HTTP status code of the response was not expected (" + status + ").", status, responseData, headers, null);
				}
			}
			finally
			{
				if (disposeResponse)
					response.Dispose();
			}
		}
		finally
		{
			if (disposeClient)
				client.Dispose();
		}
	}
    public async Task<Tuple<ICollection<UserShort>, string>> GetFollowersChunk(string userId, int? amount, string maxId, CancellationToken cancellationToken)
	{
		if (userId == null)
			throw new ArgumentNullException(nameof(userId));

		var urlBuilder = new StringBuilder();
		urlBuilder.Append(BaseUrl != null ? BaseUrl.TrimEnd('/') : "").Append("/v1/user/followers/chunk?");
		urlBuilder.Append(Uri.EscapeDataString("user_id") + "=").Append(Uri.EscapeDataString(ConvertToString(userId, CultureInfo.InvariantCulture))).Append("&");
		if (amount != null)
		{
			urlBuilder.Append(Uri.EscapeDataString("amount") + "=").Append(Uri.EscapeDataString(ConvertToString(amount, CultureInfo.InvariantCulture))).Append("&");
		}
		if (maxId != null)
		{
			urlBuilder.Append(Uri.EscapeDataString("max_id") + "=").Append(Uri.EscapeDataString(ConvertToString(maxId, CultureInfo.InvariantCulture))).Append("&");
		}
		urlBuilder.Length--;

		var client = _httpClient;
		var disposeClient = false;
		try
		{
			using var request = new HttpRequestMessage();
			request.Method = new HttpMethod("GET");
			request.Headers.Accept.Add(MediaTypeWithQualityHeaderValue.Parse("application/json"));

			PrepareRequest(client, request, urlBuilder);

			var url = urlBuilder.ToString();
			request.RequestUri = new Uri(url, UriKind.RelativeOrAbsolute);

			PrepareRequest(client, request, url);

			var response = await client.SendAsync(request, HttpCompletionOption.ResponseHeadersRead, cancellationToken).ConfigureAwait(false);
			var disposeResponse = true;
			try
			{
				var headers = response.Headers.ToDictionary(h => h.Key, h => h.Value);
				if (response.Content != null && response.Content.Headers != null)
				{
					foreach (var item in response.Content.Headers)
						headers[item.Key] = item.Value;
				}

				ProcessResponse(client, response);

				var status = (int)response.StatusCode;
				if (status == 200)
				{
					var objectResponse = await ReadObjectResponseAsync<Tuple<ICollection<UserShort>, string>>(response, headers, cancellationToken).ConfigureAwait(false);
					if (objectResponse.Object == null)
					{
						throw new ApiException("Response was null which was not expected.", status, objectResponse.Text, headers, null);
					}
					return objectResponse.Object;
				}
				else
				if (status == 404)
				{
					string responseText = (response.Content == null) ? string.Empty : await response.Content.ReadAsStringAsync().ConfigureAwait(false);
					throw new ApiException("Not found", status, responseText, headers, null);
				}
				else
				if (status == 422)
				{
					var objectResponse = await ReadObjectResponseAsync<HttpValidationError>(response, headers, cancellationToken).ConfigureAwait(false);
					if (objectResponse.Object == null)
					{
						throw new ApiException("Response was null which was not expected.", status, objectResponse.Text, headers, null);
					}
					throw new ApiException<HttpValidationError>("Validation Error", status, objectResponse.Text, headers, objectResponse.Object, null);
				}
				else
				{
					var responseData = response.Content == null ? null : await response.Content.ReadAsStringAsync().ConfigureAwait(false);
					throw new ApiException("The HTTP status code of the response was not expected (" + status + ").", status, responseData, headers, null);
				}
			}
			finally
			{
				if (disposeResponse)
					response.Dispose();
			}
		}
		finally
		{
			if (disposeClient)
				client.Dispose();
		}
	}

    
    public async Task<ICollection<UserShort>> SearchFollowings(string userId, string query, CancellationToken cancellationToken)
	{
		if (userId == null)
			throw new ArgumentNullException(nameof(userId));

		if (query == null)
			throw new ArgumentNullException(nameof(query));

		var urlBuilder = new StringBuilder();
		urlBuilder.Append(BaseUrl != null ? BaseUrl.TrimEnd('/') : "").Append("/v1/user/search/following?");
		urlBuilder.Append(Uri.EscapeDataString("user_id") + "=").Append(Uri.EscapeDataString(ConvertToString(userId, CultureInfo.InvariantCulture))).Append("&");
		urlBuilder.Append(Uri.EscapeDataString("query") + "=").Append(Uri.EscapeDataString(ConvertToString(query, CultureInfo.InvariantCulture))).Append("&");
		urlBuilder.Length--;

		var client = _httpClient;
		var disposeClient = false;
		try
		{
			using var request = new HttpRequestMessage();
			request.Method = new HttpMethod("GET");
			request.Headers.Accept.Add(MediaTypeWithQualityHeaderValue.Parse("application/json"));

			PrepareRequest(client, request, urlBuilder);

			var url = urlBuilder.ToString();
			request.RequestUri = new Uri(url, UriKind.RelativeOrAbsolute);

			PrepareRequest(client, request, url);

			var response = await client.SendAsync(request, HttpCompletionOption.ResponseHeadersRead, cancellationToken).ConfigureAwait(false);
			var disposeResponse = true;
			try
			{
				var headers = response.Headers.ToDictionary(h => h.Key, h => h.Value);
				if (response.Content != null && response.Content.Headers != null)
				{
					foreach (var item in response.Content.Headers)
						headers[item.Key] = item.Value;
				}

				ProcessResponse(client, response);

				var status = (int)response.StatusCode;
				if (status == 200)
				{
					var objectResponse = await ReadObjectResponseAsync<ICollection<UserShort>>(response, headers, cancellationToken).ConfigureAwait(false);
					if (objectResponse.Object == null)
					{
						throw new ApiException("Response was null which was not expected.", status, objectResponse.Text, headers, null);
					}
					return objectResponse.Object;
				}
				else
				if (status == 404)
				{
					string responseText = (response.Content == null) ? string.Empty : await response.Content.ReadAsStringAsync().ConfigureAwait(false);
					throw new ApiException("Not found", status, responseText, headers, null);
				}
				else
				if (status == 422)
				{
					var objectResponse = await ReadObjectResponseAsync<HttpValidationError>(response, headers, cancellationToken).ConfigureAwait(false);
					if (objectResponse.Object == null)
					{
						throw new ApiException("Response was null which was not expected.", status, objectResponse.Text, headers, null);
					}
					throw new ApiException<HttpValidationError>("Validation Error", status, objectResponse.Text, headers, objectResponse.Object, null);
				}
				else
				{
					var responseData = response.Content == null ? null : await response.Content.ReadAsStringAsync().ConfigureAwait(false);
					throw new ApiException("The HTTP status code of the response was not expected (" + status + ").", status, responseData, headers, null);
				}
			}
			finally
			{
				if (disposeResponse)
					response.Dispose();
			}
		}
		finally
		{
			if (disposeClient)
				client.Dispose();
		}
	}
    public async Task<ICollection<UserShort>> GetFollowings(string userId, int? amount, CancellationToken cancellationToken)
	{
		if (userId == null)
			throw new ArgumentNullException(nameof(userId));

		var urlBuilder = new StringBuilder();
		urlBuilder.Append(BaseUrl != null ? BaseUrl.TrimEnd('/') : "").Append("/v1/user/following?");
		urlBuilder.Append(Uri.EscapeDataString("user_id") + "=").Append(Uri.EscapeDataString(ConvertToString(userId, CultureInfo.InvariantCulture))).Append("&");
		if (amount != null)
		{
			urlBuilder.Append(Uri.EscapeDataString("amount") + "=").Append(Uri.EscapeDataString(ConvertToString(amount, CultureInfo.InvariantCulture))).Append("&");
		}
		urlBuilder.Length--;

		var client = _httpClient;
		var disposeClient = false;
		try
		{
			using var request = new HttpRequestMessage();
			request.Method = new HttpMethod("GET");
			request.Headers.Accept.Add(MediaTypeWithQualityHeaderValue.Parse("application/json"));

			PrepareRequest(client, request, urlBuilder);

			var url = urlBuilder.ToString();
			request.RequestUri = new Uri(url, UriKind.RelativeOrAbsolute);

			PrepareRequest(client, request, url);

			var response = await client.SendAsync(request, HttpCompletionOption.ResponseHeadersRead, cancellationToken).ConfigureAwait(false);
			var disposeResponse = true;
			try
			{
				var headers = response.Headers.ToDictionary(h => h.Key, h => h.Value);
				if (response.Content != null && response.Content.Headers != null)
				{
					foreach (var item in response.Content.Headers)
						headers[item.Key] = item.Value;
				}

				ProcessResponse(client, response);

				var status = (int)response.StatusCode;
				if (status == 200)
				{
					var objectResponse = await ReadObjectResponseAsync<ICollection<UserShort>>(response, headers, cancellationToken).ConfigureAwait(false);
					if (objectResponse.Object == null)
					{
						throw new ApiException("Response was null which was not expected.", status, objectResponse.Text, headers, null);
					}
					return objectResponse.Object;
				}
				else
				if (status == 404)
				{
					string responseText = (response.Content == null) ? string.Empty : await response.Content.ReadAsStringAsync().ConfigureAwait(false);
					throw new ApiException("Not found", status, responseText, headers, null);
				}
				else
				if (status == 422)
				{
					var objectResponse = await ReadObjectResponseAsync<HttpValidationError>(response, headers, cancellationToken).ConfigureAwait(false);
					if (objectResponse.Object == null)
					{
						throw new ApiException("Response was null which was not expected.", status, objectResponse.Text, headers, null);
					}
					throw new ApiException<HttpValidationError>("Validation Error", status, objectResponse.Text, headers, objectResponse.Object, null);
				}
				else
				{
					var responseData = response.Content == null ? null : await response.Content.ReadAsStringAsync().ConfigureAwait(false);
					throw new ApiException("The HTTP status code of the response was not expected (" + status + ").", status, responseData, headers, null);
				}
			}
			finally
			{
				if (disposeResponse)
					response.Dispose();
			}
		}
		finally
		{
			if (disposeClient)
				client.Dispose();
		}
	}
    public async Task<Tuple<ICollection<UserShort>, string>> GetFollowingsChunk(string userId, int? amount, string maxId, CancellationToken cancellationToken)
	{
		if (userId == null)
			throw new ArgumentNullException(nameof(userId));

		var urlBuilder = new StringBuilder();
		urlBuilder.Append(BaseUrl != null ? BaseUrl.TrimEnd('/') : "").Append("/v1/user/following/chunk?");
		urlBuilder.Append(Uri.EscapeDataString("user_id") + "=").Append(Uri.EscapeDataString(ConvertToString(userId, CultureInfo.InvariantCulture))).Append("&");
		if (amount != null)
		{
			urlBuilder.Append(Uri.EscapeDataString("amount") + "=").Append(Uri.EscapeDataString(ConvertToString(amount, CultureInfo.InvariantCulture))).Append("&");
		}
		if (maxId != null)
		{
			urlBuilder.Append(Uri.EscapeDataString("max_id") + "=").Append(Uri.EscapeDataString(ConvertToString(maxId, CultureInfo.InvariantCulture))).Append("&");
		}
		urlBuilder.Length--;

		var client = _httpClient;
		var disposeClient = false;
		try
		{
			using var request = new HttpRequestMessage();
			request.Method = new HttpMethod("GET");
			request.Headers.Accept.Add(MediaTypeWithQualityHeaderValue.Parse("application/json"));

			PrepareRequest(client, request, urlBuilder);

			var url = urlBuilder.ToString();
			request.RequestUri = new Uri(url, UriKind.RelativeOrAbsolute);

			PrepareRequest(client, request, url);

			var response = await client.SendAsync(request, HttpCompletionOption.ResponseHeadersRead, cancellationToken).ConfigureAwait(false);
			var disposeResponse = true;
			try
			{
				var headers = response.Headers.ToDictionary(h => h.Key, h => h.Value);
				if (response.Content != null && response.Content.Headers != null)
				{
					foreach (var item in response.Content.Headers)
						headers[item.Key] = item.Value;
				}

				ProcessResponse(client, response);

				var status = (int)response.StatusCode;
				if (status == 200)
				{
					var objectResponse = await ReadObjectResponseAsync<Tuple<ICollection<UserShort>, string>>(response, headers, cancellationToken).ConfigureAwait(false);
					if (objectResponse.Object == null)
					{
						throw new ApiException("Response was null which was not expected.", status, objectResponse.Text, headers, null);
					}
					return objectResponse.Object;
				}
				else
				if (status == 404)
				{
					string responseText = (response.Content == null) ? string.Empty : await response.Content.ReadAsStringAsync().ConfigureAwait(false);
					throw new ApiException("Not found", status, responseText, headers, null);
				}
				else
				if (status == 422)
				{
					var objectResponse = await ReadObjectResponseAsync<HttpValidationError>(response, headers, cancellationToken).ConfigureAwait(false);
					if (objectResponse.Object == null)
					{
						throw new ApiException("Response was null which was not expected.", status, objectResponse.Text, headers, null);
					}
					throw new ApiException<HttpValidationError>("Validation Error", status, objectResponse.Text, headers, objectResponse.Object, null);
				}
				else
				{
					var responseData = response.Content == null ? null : await response.Content.ReadAsStringAsync().ConfigureAwait(false);
					throw new ApiException("The HTTP status code of the response was not expected (" + status + ").", status, responseData, headers, null);
				}
			}
			finally
			{
				if (disposeResponse)
					response.Dispose();
			}
		}
		finally
		{
			if (disposeClient)
				client.Dispose();
		}
	}
}

public class FakeUser
{
	[JsonProperty("pk", Required = Required.Default)]
	public string Pk { private get; set; } = null;

	[JsonProperty("id", Required = Required.Default)]
	public string Id { private get; set; } = null;

	public string Key()
	{
		if (!string.IsNullOrEmpty(Pk))
		{
			return Pk;
		}
		
		if (!string.IsNullOrEmpty(Id))
		{
			return Id;
		}

		throw new ArgumentNullException();
	}
	
	[JsonProperty("username", Required = Required.Always)]
	[Required(AllowEmptyStrings = true)]
	public string Username { get; set; }

	[JsonProperty("full_name", Required = Required.DisallowNull, NullValueHandling = NullValueHandling.Ignore)]
	public string FullName { get; set; }

	[JsonProperty("edge_followed_by", Required = Required.DisallowNull, NullValueHandling = NullValueHandling.Ignore)]
	public Countable FollowerCount { get; set; }

	[JsonProperty("edge_follow", Required = Required.DisallowNull, NullValueHandling = NullValueHandling.Ignore)]
	public Countable FollowingCount { get; set; }
}

public class Countable 
{
	[JsonProperty("count", Required = Required.DisallowNull, NullValueHandling = NullValueHandling.Ignore)]
	public int Count { get; set; }
}


[GeneratedCode("NJsonSchema", "13.18.2.0 (NJsonSchema v10.8.0.0 (Newtonsoft.Json v13.0.0.0))")]
public class User
{
	[JsonProperty("pk", Required = Required.Always)]
	[Required(AllowEmptyStrings = true)]
	public string Pk { get; set; }

	[JsonProperty("username", Required = Required.Always)]
	[Required(AllowEmptyStrings = true)]
	public string Username { get; set; }

	[JsonProperty("full_name", Required = Required.DisallowNull, NullValueHandling = NullValueHandling.Ignore)]
	public string FullName { get; set; }

	[JsonProperty("is_private", Required = Required.DisallowNull, NullValueHandling = NullValueHandling.Ignore)]
	public bool IsPrivate { get; set; }

	[JsonProperty("profile_pic_url", Required = Required.Always)]
	[Required]
	[StringLength(2083, MinimumLength = 1)]
	public Uri ProfilePicUrl { get; set; }

	[JsonProperty("profile_pic_url_hd", Required = Required.DisallowNull, NullValueHandling = NullValueHandling.Ignore)]
	[StringLength(2083, MinimumLength = 1)]
	public Uri ProfilePicUrlHd { get; set; }

	[JsonProperty("is_verified", Required = Required.DisallowNull, NullValueHandling = NullValueHandling.Ignore)]
	public bool IsVerified { get; set; }

	[JsonProperty("media_count", Required = Required.DisallowNull, NullValueHandling = NullValueHandling.Ignore)]
	public int MediaCount { get; set; }

	[JsonProperty("follower_count", Required = Required.DisallowNull, NullValueHandling = NullValueHandling.Ignore)]
	public int FollowerCount { get; set; }

	[JsonProperty("following_count", Required = Required.DisallowNull, NullValueHandling = NullValueHandling.Ignore)]
	public int FollowingCount { get; set; }

	[JsonProperty("biography", Required = Required.DisallowNull, NullValueHandling = NullValueHandling.Ignore)]
	public string Biography { get; set; } = "";

	[JsonProperty("external_url", Required = Required.DisallowNull, NullValueHandling = NullValueHandling.Ignore)]
	public string ExternalUrl { get; set; }

	[JsonProperty("account_type", Required = Required.DisallowNull, NullValueHandling = NullValueHandling.Ignore)]
	public int AccountType { get; set; }

	[JsonProperty("is_business", Required = Required.DisallowNull, NullValueHandling = NullValueHandling.Ignore)]
	public bool IsBusiness { get; set; }

	[JsonProperty("public_email", Required = Required.DisallowNull, NullValueHandling = NullValueHandling.Ignore)]
	public string PublicEmail { get; set; }

	[JsonProperty("contact_phone_number", Required = Required.DisallowNull, NullValueHandling = NullValueHandling.Ignore)]
	public string ContactPhoneNumber { get; set; }

	[JsonProperty("public_phone_country_code", Required = Required.DisallowNull, NullValueHandling = NullValueHandling.Ignore)]
	public string PublicPhoneCountryCode { get; set; }

	[JsonProperty("public_phone_number", Required = Required.DisallowNull, NullValueHandling = NullValueHandling.Ignore)]
	public string PublicPhoneNumber { get; set; }

	[JsonProperty("business_contact_method", Required = Required.DisallowNull, NullValueHandling = NullValueHandling.Ignore)]
	public string BusinessContactMethod { get; set; }

	[JsonProperty("business_category_name", Required = Required.DisallowNull, NullValueHandling = NullValueHandling.Ignore)]
	public string BusinessCategoryName { get; set; }

	[JsonProperty("category_name", Required = Required.DisallowNull, NullValueHandling = NullValueHandling.Ignore)]
	public string CategoryName { get; set; }

	[JsonProperty("category", Required = Required.DisallowNull, NullValueHandling = NullValueHandling.Ignore)]
	public string Category { get; set; }

	[JsonProperty("address_street", Required = Required.DisallowNull, NullValueHandling = NullValueHandling.Ignore)]
	public string AddressStreet { get; set; }

	[JsonProperty("city_id", Required = Required.DisallowNull, NullValueHandling = NullValueHandling.Ignore)]
	public string CityId { get; set; }

	[JsonProperty("city_name", Required = Required.DisallowNull, NullValueHandling = NullValueHandling.Ignore)]
	public string CityName { get; set; }

	[JsonProperty("latitude", Required = Required.DisallowNull, NullValueHandling = NullValueHandling.Ignore)]
	public double Latitude { get; set; }

	[JsonProperty("longitude", Required = Required.DisallowNull, NullValueHandling = NullValueHandling.Ignore)]
	public double Longitude { get; set; }

	[JsonProperty("zip", Required = Required.DisallowNull, NullValueHandling = NullValueHandling.Ignore)]
	public string Zip { get; set; }

	[JsonProperty("instagram_location_id", Required = Required.DisallowNull, NullValueHandling = NullValueHandling.Ignore)]
	public string InstagramLocationId { get; set; }

	[JsonProperty("interop_messaging_user_fbid", Required = Required.DisallowNull, NullValueHandling = NullValueHandling.Ignore)]
	public string InteropMessagingUserFbid { get; set; }

	private IDictionary<string, object> _additionalProperties;

	[JsonExtensionData]
	public IDictionary<string, object> AdditionalProperties
	{
		get => _additionalProperties ??= new Dictionary<string, object>();
		set => _additionalProperties = value;
	}

}

[GeneratedCode("NJsonSchema", "13.18.2.0 (NJsonSchema v10.8.0.0 (Newtonsoft.Json v13.0.0.0))")]
public class UserShort
{
	[JsonProperty("pk", Required = Required.Always)]
	[Required(AllowEmptyStrings = true)]
	public string Pk { get; set; }

	[JsonProperty("username", Required = Required.DisallowNull, NullValueHandling = NullValueHandling.Ignore)]
	public string Username { get; set; }

	[JsonProperty("full_name", Required = Required.DisallowNull, NullValueHandling = NullValueHandling.Ignore)]
	public string FullName { get; set; } = "";

	[JsonProperty("profile_pic_url", Required = Required.DisallowNull, NullValueHandling = NullValueHandling.Ignore)]
	[StringLength(2083, MinimumLength = 1)]
	public Uri ProfilePicUrl { get; set; }

	[JsonProperty("profile_pic_url_hd", Required = Required.DisallowNull, NullValueHandling = NullValueHandling.Ignore)]
	[StringLength(2083, MinimumLength = 1)]
	public Uri ProfilePicUrlHd { get; set; }

	[JsonProperty("is_private", Required = Required.DisallowNull, NullValueHandling = NullValueHandling.Ignore)]
	public bool IsPrivate { get; set; }

	[JsonProperty("is_verified", Required = Required.DisallowNull, NullValueHandling = NullValueHandling.Ignore)]
	public bool IsVerified { get; set; }

	[JsonProperty("stories", Required = Required.DisallowNull, NullValueHandling = NullValueHandling.Ignore)]
	public ICollection<object> Stories { get; set; }

	private IDictionary<string, object> _additionalProperties;

	[JsonExtensionData]
	public IDictionary<string, object> AdditionalProperties
	{
		get => _additionalProperties ??= new Dictionary<string, object>();
		set => _additionalProperties = value;
	}
}

[GeneratedCode("NSwag", "13.18.2.0 (NJsonSchema v10.8.0.0 (Newtonsoft.Json v13.0.0.0))")]
public class ApiException : Exception
{
	public int StatusCode { get; private set; }

	public string Response { get; private set; }

	public IReadOnlyDictionary<string, IEnumerable<string>> Headers { get; private set; }

	public ApiException(string message, int statusCode, string response, IReadOnlyDictionary<string, IEnumerable<string>> headers, Exception innerException)
		: base(message + "\n\nStatus: " + statusCode + "\nResponse: \n" + ((response == null) ? "(null)" : response.Substring(0, response.Length >= 512 ? 512 : response.Length)), innerException)
	{
		StatusCode = statusCode;
		Response = response;
		Headers = headers;
	}

	public override string ToString()
	{
		return $"HTTP Response: \n\n{Response}\n\n{base.ToString()}";
	}
}

[GeneratedCode("NSwag", "13.18.2.0 (NJsonSchema v10.8.0.0 (Newtonsoft.Json v13.0.0.0))")]
public class ApiException<TResult> : ApiException
{
	public TResult Result { get; private set; }

	public ApiException(string message, int statusCode, string response, IReadOnlyDictionary<string, IEnumerable<string>> headers, TResult result, Exception innerException)
		: base(message, statusCode, response, headers, innerException)
	{
		Result = result;
	}
}

[GeneratedCode("NJsonSchema", "13.18.2.0 (NJsonSchema v10.8.0.0 (Newtonsoft.Json v13.0.0.0))")]
public class HttpValidationError
{
	[JsonProperty("detail", Required = Required.DisallowNull, NullValueHandling = NullValueHandling.Ignore)]
	public ICollection<ValidationError> Detail { get; set; }

	private IDictionary<string, object> _additionalProperties;

	[JsonExtensionData]
	public IDictionary<string, object> AdditionalProperties
	{
		get => _additionalProperties ??= new Dictionary<string, object>();
		set => _additionalProperties = value;
	}
}

[GeneratedCode("NJsonSchema", "13.18.2.0 (NJsonSchema v10.8.0.0 (Newtonsoft.Json v13.0.0.0))")]
public class ValidationError
{
	[JsonProperty("loc", Required = Required.Always)]
	[Required]
	public ICollection<Loc> Loc { get; set; } = new Collection<Loc>();

	[JsonProperty("msg", Required = Required.Always)]
	[Required(AllowEmptyStrings = true)]
	public string Msg { get; set; }

	[JsonProperty("type", Required = Required.Always)]
	[Required(AllowEmptyStrings = true)]
	public string Type { get; set; }

	private IDictionary<string, object> _additionalProperties;

	[JsonExtensionData]
	public IDictionary<string, object> AdditionalProperties
	{
		get => _additionalProperties ??= new Dictionary<string, object>();
		set => _additionalProperties = value;
	}
}

[GeneratedCode("NJsonSchema", "13.18.2.0 (NJsonSchema v10.8.0.0 (Newtonsoft.Json v13.0.0.0))")]
public class Loc
{
	private IDictionary<string, object> _additionalProperties;

	[JsonExtensionData]
	public IDictionary<string, object> AdditionalProperties
	{
		get => _additionalProperties ??= new Dictionary<string, object>();
		set => _additionalProperties = value;
	}
}