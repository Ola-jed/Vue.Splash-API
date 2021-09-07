# Vue.Splash-API

Rest api written with Asp.Net Core for an [unsplash](https://unsplash.com/) clone

## Requirements

[Dotnet](https://dotnet.microsoft.com/download)

> [Postgres](https://www.postgresql.org/) is the used dbms
>
> Create the database `splash` in postgres and add the _**UserId**_, _**Password**_ credentials for the database
> 
> Add the _**KeyId**_, _**AppKey**_, _**BucketId**_ credentials for Backblaze 

## Setup

```shell
git clone https://github.com/Ola-jed/Vue.Splash-API.git
cd Vue.Splash-API/Vue.Splash-API
dotnet run
```
You can also try the [frontend](https://github.com/tobihans/Vue.Splash) built by [@tobihans](https://github.com/tobihans)

## TODO

- [ ] Finish LocalStorageService implementation
  - [x] Delete a local file
  - [ ] Update a local file
- [x] Make an custom client for [Backblaze](https://www.backblaze.com/)
- [ ] Drive implementation ?
- [ ] Password reset (import [DotWiki](https://github.com/Ola-jed/DotWikiApi) mail client)
- [ ] Unit tests