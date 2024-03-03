for /d %%i in (*) do (
	cd %%i
	start dotnet build --configuration=release
	cd ..
)