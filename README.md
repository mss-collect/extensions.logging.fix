# MSS.Extensions.Configuration.Fix

## History

Date|Version|Description
---|---|---
29.10.2021|0.1.0|first release
27.09.2022|1.0.0|NuGet & .Net update (5 => 6) and readme update
27.09.2022|1.1.0|Improve extensions for Azure Functions >= v4
16.11.2022|1.2.0|Improve handling if there are more then one environment sources, NuGet updates

## Usage

Ensure that the following Extension Method is called!

Namespace: Microsoft.Extensions.Configuration


```csharp
public static IWebHostBuilder ConfigureConfigurationFix(this IWebHostBuilder hostBuilder)
```
