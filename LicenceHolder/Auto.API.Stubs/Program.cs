var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapPost("/api/NotifySendMail", () => "{\r\n    \"ExistingUser\": false,\r\n    \"ActivationUrl\": \"https://something.com\",\r\n    \"User\": {\r\n        \"Id\": \"something-made-up\",\r\n        \"Status\": \"Hllo\"\r\n    }\r\n}");

app.MapPost("/api/OktaEnsureUser", () => "{\n    \"ExistingUser\": true,\n    \"ActivationUrl\": \"https://dev-43591058.okta.com/tokens/jkdsa9809asdjklasdds9a/verify\",\n    \"User\": {\n        \"Id\": \"kksjfdhlahdflhihlhsdffd924\",\n        \"Status\": \"PROVISIONED\",\n        \"FirstName\": \"Joe\",\n        \"LastName\": \"Blogs\",\n        \"Email\": \"jb@__.uk\",\n        \"Login\": \"jb@__.uk\",\n        \"Organization\": \"ACME Inc\",\n        \"PrimaryPhone\": \"111\",\n        \"Created\": \"2024-04-16T14:58:39+00:00\",\n        \"LastUpdated\": \"2024-04-19T10:41:19+00:00\"\n    }\n}");

app.MapPost("/api/OktaRemoveUserFromGroup", () => "");

app.Run();
