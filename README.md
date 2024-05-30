# Kameleoon .NET Starter Kit

> The Kameleoon .NET Starter Kit demonstrates how to use Kameleoon experimentation and feature flags with the C# SDK.

This repository is home to the Kameleoon starter kit for .NET. Kameleoon is a powerful experimentation and personalization platform for product teams that enables you to make insightful discoveries by testing the features on your roadmap. Discover more at https://kameleoon.com, or see the [developer documentation](https://developers.kameleoon.com).

## Using the Starter Kit

This starter kit provides quickstart instructions for developers using the [Kameleoon .NET SDK](https://developers.kameleoon.com/feature-management-and-experimentation/web-sdks/csharp-sdk/).

### Prerequisites

Make sure you have the following requirements:

1. A Kameleoon user account. Visit [kameleoon.com](https://www.kameleoon.com/) to learn more.
2. .NET version 7.0 (or later) installed.

### Install

You can now test a number of the SDK methods to better understand how the SDK works. Follow these steps to install the Starter Kit:

1. Clone this repository and move into the cloned directory:

```bash
git clone git@github.com:Kameleoon/starter-kit-dotnet.git
cd start-kit-dotnet
```

2. In the `Program.cs` file, set your `clientId`, `clientSecret`, and `siteCode`. To start the server locally, leave `localhost` as `topLevelDomain`. If you want to deploy on remote server, set the `topLevelDomain` to your actual top-level domain:

```csharp
var config = new KameleoonClientConfig("clientId", "clientSecret", topLevelDomain: "localhost");
var client = KameleoonClientFactory.Create("siteCode", config);
```

3. Run the server. By default, the server runs locally on port `4300`:

```bash
dotnet run
```

### Basic usage

Here is a list of the typical ways you might use our SDK:

1. Add targeting and retrieve a variation key for a visitor:
```
http://localhost:4300/Basic?index=<index>&value=<value>&featureKey=<featureKey>
```

2. Fetch remote data for a visitor and get a variable value for the assigned variation.
```
http://localhost:4300/Remote?featureKey=<featureKey>&variableKey=<variableKey>
```

3. Get all feature flags and check whether or not they are active for a visitor.
```
http://localhost:4300/AllFlags
```

### Advanced Usage

The SDK provides additional methods that you can test:

1. Get a list of keys for available feature flags:
```
http://localhost:4300/FeatureList
```

2. Add data for the visitor and flush the data to the Kameleoon Data API:
```
http://localhost:4300/AddData?index=<index>&value=<value>
```

3. Get the active feature flags for the visitor:
```
http://localhost:4300/ActiveFeatures
```

4. Check whether or not a feature is active for the visitor:
```
http://localhost:4300/FeatureActive?featureKey=<featureKey>
```

5. Get the variation key of the assigned variation for the visitor:
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
