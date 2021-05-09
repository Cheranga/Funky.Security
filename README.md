# Funky.Security

### Todo
- [ ] Write tests.


### References

* Creating your own custom Azure function binding

https://github.com/ealsur/functions-extension-101


* Could not load file or assembly System.IdentityModel.Tokens.Jwt

To fix this you will need to modify the project properties as mentioned in this link.
https://github.com/Azure/azure-functions-vs-build-sdk/issues/397

The change to do is to add the below,
```xml
<PropertyGroup>
    <TargetFramework>netcoreapp3.1</TargetFramework>
    <AzureFunctionsVersion>V3</AzureFunctionsVersion>
    <_FunctionsSkipCleanOutput>true</_FunctionsSkipCleanOutput>
</PropertyGroup>
```

Notice the `_FunctionsSkipCleanOutput` tag with an "underscore".

* Creating and validating custom JWT tokens
  
https://jasonwatmore.com/post/2020/07/21/aspnet-core-3-create-and-validate-jwt-tokens-use-custom-jwt-middleware


* Manually validating an Azure AD token

https://gist.github.com/Tanver-Hasan/d4b116d4dad2a4899aa34bf2222bfaed



