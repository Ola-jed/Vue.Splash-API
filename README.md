# Vue.Splash-API

Rest api written with Asp.Net Core for an [unsplash](https://unsplash.com/) clone

## Requirements

- [Dotnet](https://dotnet.microsoft.com/download)
- [Postgres](https://www.postgresql.org/)
- [A Backblaze account](https://www.backblaze.com/)
- [A Gmail account](gmail.com) (If you want to test some parts involving email sending)
>
> Create the database `splash` in postgres and set _**UserId**_ and _**Password**_ environment variables for the database credentials.
>
> Set _**KeyId**_, _**AppKey**_ and _**BucketId**_ environment variables for Backblaze credentials.
> 
> Set _**MailUser**_ and _**MailPassword**_ for email sender credentials.

## Setup

```shell
git clone https://github.com/Ola-jed/Vue.Splash-API.git
cd Vue.Splash-API/Vue.Splash-API
# Set the env vars before
# export X=xxx
dotnet ef database update
dotnet run
```

You can also try the [frontend](https://github.com/tobihans/Vue.Splash) built by [@tobihans](https://github.com/tobihans)

## TODO

- [x] Finish LocalStorageService implementation
  - [x] Delete a local file
- [x] Make a custom client for [Backblaze](https://www.backblaze.com/)
- [ ] Drive implementation ??
- [x] Password reset (import [DotWiki](https://github.com/Ola-jed/DotWikiApi) mail client)
- [ ] Unit tests
