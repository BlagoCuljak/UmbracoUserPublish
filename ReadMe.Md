If you set 0 as user id, that is not existing, you can publish content.

Use email and password to login. Go to published content and you will see the post, but on Info, you will get an error.
```
blago@green-code.dev
Umbraco123
```

Use CURL to test the API.
```
curl --location 'http://localhost:34433/api/newpost' \
--header 'Content-Type: application/json' \
--data '{
  "Name": "Set with not existing user",
  "Text": "This is the content of the sample post."
}'


```