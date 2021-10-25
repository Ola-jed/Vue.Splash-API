# Vue.Splash-API

Rest api written with Asp.Net Core for an [unsplash](https://unsplash.com/) clone

## Requirements

- [Dotnet](https://dotnet.microsoft.com/download)
- [Postgres](https://www.postgresql.org/)
- [A Backblaze account](https://www.backblaze.com/) or a [Microsoft Azure account](https://portal.azure.com/)
>
> Create the database `splash` in postgres and set _**UserId**_ and _**Password**_ environment variables for the database credentials.
>
> Set _**KeyId**_, _**AppKey**_ and _**BucketId**_ environment variables for Backblaze credentials.
>
> Set _**MailUser**_, _**MailPassword**_ and optionally _**MailHost**_ and  _**MailPort**_ for environment variables for mail config.
> 
> Set _**AzureBlobKey**_ which is the access key for azure storage account and _**ContainerName**_ which is the name of the container you will use to store the files

## Setup

```shell
git clone https://github.com/Ola-jed/Vue.Splash-API.git
cd Vue.Splash-API/Vue.Splash-API
# Set the env vars before
# export X=xxx or dotnet user-secrets set "Key" "Value"
dotnet ef database update
dotnet run
```

You can also try the [frontend](https://github.com/tobihans/Vue.Splash) built by [@tobihans](https://github.com/tobihans)

## TODO

- [x] Finish LocalStorageService implementation
  - [x] Delete a local file
- [x] Make an custom client for [Backblaze](https://www.backblaze.com/)
- [x] Password reset (import [DotWiki](https://github.com/Ola-jed/DotWikiApi) mail client)
- [ ] Unit tests

> If you cannot setup backblaze or azure, you can swap the storage service to use  by `LocalStorageService` in the file `Startup.cs` (line 40)