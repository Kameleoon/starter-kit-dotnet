# Kameleoon .NET Starter Kit

> The Kameleoon .NET Starter Kit demonstrates Kameleoon experimentation and feature flags.

This repository is home to the Kameleoon starter kit for .NET. Kameleoon is a powerful experimentation and personalization platform for product teams that enables you to make insightful discoveries by testing the features on your roadmap. Discover more at https://kameleoon.com, or see the [developer documentation](https://developers.kameleoon.com).

## How to use

### Before get started

This starter kit provides quickstart instructions for developers using the [Kameleoon .NET SDK](https://developers.kameleoon.com/feature-management-and-experimentation/web-sdks/csharp-sdk/).

### Prerequisites

Make sure you have the following requirements before you get started:

1. A Kameleoon user account. Visit [kameleoon.com](https://www.kameleoon.com/) to learn more.
2. Have the `.NET 7.0` (or later) installed.

### Get started

1. Clone the current repository and move into the directory:

```bash
git clone git@github.com:Kameleoon/starter-kit-dotnet.git
cd start-kit-dotnet
```

2. Set `clientId` / `clientSecret`, `siteCode` in `Program.cs` file:

```csharp
var config = new KameleoonClientConfig("clientId", "clientSecret", topLevelDomain: "localhost");
var client = KameleoonClientFactory.Create("siteCode", config);
```

We're using `localhost` as `topLevelDomain` because server will be started locally, if you want to deploy on remote server you should set real `topLevelDomain` value.

3. Run a server locally on `4300` port:

```bash
dotnet run
```

### Basic Usage

> Here is a list of typical ways clients use our SDK:

1. Add targeting add and retrieve a variation key for a visitor:
```
http://localhost:4300/Basic?index=<index>&value=<value>&featureKey=<featureKey>
```

2. Fetch remote data for a visitor and get variable value for the assigned variation.
```
http://localhost:4300/Remote?featureKey=<featureKey>&variableKey=<variableKey>
```

3. Get all feature flags and check if they are active for a visitor or not.
```
http://localhost:4300/AllFlags
```

### Specific Usage

> The SDK methods are divided into separate calls for easier understanding and testing if necessary.

1. Get list of available feature flag's keys:
```
http://localhost:4300/FeatureList
```

2. Add data for visitor and flush to Kameleoon Data API:
```
http://localhost:4300/AddData?index=<index>&value=<value>
```

3. Get active feature flag's for a visitor:
```
http://localhost:4300/ActiveFeatures
```

4. Check if feature is active for a visitor:
```
http://localhost:4300/FeatureActive?featureKey=<featureKey>
```

5. Get a variation key of assigned variation for a visitor:
```
http://localhost:4300/FeatureVariationKey?featureKey=<featureKey>
```

6. Get a variable value key of assigned variation for a visitor:
```
http://localhost:4300/FeatureVariable?featureKey=<featureKey>&variableKey=<variableKey>
```

7. Get engine tracking code of assigned variation for a visitor:
```
http://localhost:4300/EngineTrackingCode?featurekey=<featureKey>
```

8. Get remote data of a visitor
```
http://localhost:4300/RemoteVisitorData
```
