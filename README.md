# yamlscaffold
Update nuget package after changes by running

`dotnet pack`

Install package globally

`dotnet tool install --global --add-source ./nupkg yamlscaffold.cli`

After code changes
```
dotnet build
dotnet pack
dotnet tool update -g --add-source ./nupkg yamlscaffold.cli
```
