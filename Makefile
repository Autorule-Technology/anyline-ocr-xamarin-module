# BUILD

android-sdk:
	@nuget restore BindingSource/AnylineXamarinSDK.Droid/AnylineXamarinSDK.Droid.csproj
	
	@sed -i '' "s/^\[assembly: AssemblyVersion.*/\[assembly: AssemblyVersion(\"$(MAJOR_VERSION).$(MINOR_VERSION).$(BUILD_NUMBER)\")\]/" BindingSource/AnylineXamarinSDK.Droid/Properties/AssemblyInfo.cs
ifeq ($(OS),Windows_NT)
    # Build Android DLL
	@msbuild.exe /p:Configuration="Release" \
		/p:Platform="AnyCPU" \
		/v:minimal \
		/t:rebuild "BindingSource/AnylineXamarinSDK.Droid/AnylineXamarinSDK.Droid.csproj"
    # Build once more to generate the Metadata
	@msbuild.exe /p:Configuration="Release" \
		/p:Platform="AnyCPU" \
		/v:minimal \
		/t:rebuild "BindingSource/AnylineXamarinSDK.Droid/AnylineXamarinSDK.Droid.csproj"
else
    # Build Android DLL
	@msbuild /p:Configuration="Release" \
		/p:Platform="AnyCPU" \
		/v:minimal \
		/t:rebuild "BindingSource/AnylineXamarinSDK.Droid/AnylineXamarinSDK.Droid.csproj"
    # Build once more to generate the Metadata
	@msbuild /p:Configuration="Release" \
		/p:Platform="AnyCPU" \
		/v:minimal \
		/t:rebuild "BindingSource/AnylineXamarinSDK.Droid/AnylineXamarinSDK.Droid.csproj"
endif
	@rm -rf Nuget/Anyline.Xamarin.SDK.Droid.*.nupkg

	@-dotnet nuget remove source LocalNuGet
	@dotnet nuget add source $(PWD)/Nuget -n LocalNuGet

	@sed -i '' "s/<version>.*/<version>$(MAJOR_VERSION).$(MINOR_VERSION).$(BUILD_NUMBER)<\/version>/" Nuget/Anyline.Xamarin.SDK.Droid.nuspec
	@nuget pack Nuget/Anyline.Xamarin.SDK.Droid.nuspec -OutputDirectory Nuget

android-examples: reference-newest-android-nuget
	@rm -rf *.apk
	@nuget restore Examples/AnylineExamples.Droid/AnylineExamples.Droid.csproj
ifeq ($(OS),Windows_NT)
	# Build the examples .apk file 
	@msbuild.exe /p:Configuration="Release" \
		/p:MonoSymbolArchive=False \
		/v:minimal \
		/t:PackageForAndroid "Examples/AnylineExamples.Droid/AnylineExamples.Droid.csproj"
else
	# Build the examples .apk file 
	@msbuild /p:Configuration="Release" \
		/p:MonoSymbolArchive=False \
		/v:minimal \
		/t:PackageForAndroid "Examples/AnylineExamples.Droid/AnylineExamples.Droid.csproj"
endif
	@echo Android Examples APK is built

	# sign the apk
	@jarsigner -sigalg MD5withRSA -digestalg SHA1 -keystore "$(KEYSTORE_PATH)" -storepass "$(KEYSTORE_RELEASE_PW)" -signedjar "Examples/AnylineExamples.Droid/bin/Release/com.anyline.xamarin.examples-Signed.apk" "Examples/AnylineExamples.Droid/bin/Release/com.anyline.xamarin.examples.apk" "Xamarin.App.Droid Sample App"
	# zipalign the apk
	@zipalign -f 4 "Examples/AnylineExamples.Droid/bin/Release/com.anyline.xamarin.examples-Signed.apk" "Examples/AnylineExamples.Droid/bin/Release/com.anyline.xamarin.examples.apk"
	
	@mv "Examples/AnylineExamples.Droid/bin/Release/com.anyline.xamarin.examples.apk" "com.anyline.xamarin.examples_$(MAJOR_VERSION).$(MINOR_VERSION).$(BUILD_NUMBER).apk"


ios-sdk:
	# Needs to be run on a Mac

	@sed -i '' "s/^\[assembly: AssemblyVersion.*/\[assembly: AssemblyVersion(\"$(MAJOR_VERSION).$(MINOR_VERSION).$(BUILD_NUMBER)\")\]/" BindingSource/AnylineXamarinSDK.iOS/Properties/AssemblyInfo.cs

	@msbuild /p:Configuration="Release" \
		/p:Platform="AnyCPU" \
		/p:BuildIpa=false \
		/v:minimal \
		/t:rebuild "BindingSource/AnylineXamarinSDK.iOS/AnylineXamarinSDK.iOS.csproj"
	
	@rm -rf Nuget/Anyline.Xamarin.SDK.iOS.*.nupkg
	
	@-dotnet nuget remove source LocalNuGet
	@dotnet nuget add source $(PWD)/Nuget -n LocalNuGet

	@sed -i '' "s/<version>.*/<version>$(MAJOR_VERSION).$(MINOR_VERSION).$(BUILD_NUMBER)<\/version>/" Nuget/Anyline.Xamarin.SDK.iOS.nuspec
	@nuget pack Nuget/Anyline.Xamarin.SDK.iOS.nuspec -OutputDirectory Nuget

ios-examples: reference-newest-ios-nuget
	# Needs to be run on a Mac
	@nuget restore Examples/AnylineExamples.iOS/AnylineExamples.iOS.csproj
	@msbuild /p:Configuration="Release" \
		/p:Platform="iPhone" \
		/p:BuildIpa=false \
		/v:minimal \
		/t:rebuild "Examples/AnylineExamples.iOS/AnylineExamples.iOS.csproj"

reference-newest-android-nuget:
	@-dotnet remove Examples/AnylineExamples.Droid/AnylineExamples.Droid.csproj package Anyline.Xamarin.SDK.Droid
	@-dotnet remove Examples/Xamarin.Forms/Anyline/Anyline.Android/Anyline.Android.csproj package Anyline.Xamarin.SDK.Droid
	@dotnet add Examples/AnylineExamples.Droid/AnylineExamples.Droid.csproj package Anyline.Xamarin.SDK.Droid
	@dotnet add Examples/Xamarin.Forms/Anyline/Anyline.Android/Anyline.Android.csproj package Anyline.Xamarin.SDK.Droid

reference-newest-ios-nuget:
	@-dotnet remove Examples/AnylineExamples.iOS/AnylineExamples.iOS.csproj package Anyline.Xamarin.SDK.iOS
	@-dotnet remove Examples/Xamarin.Forms/Anyline/Anyline.iOS/Anyline.iOS.csproj package Anyline.Xamarin.SDK.iOS
	@dotnet add Examples/AnylineExamples.iOS/AnylineExamples.iOS.csproj package Anyline.Xamarin.SDK.iOS
	@dotnet add Examples/Xamarin.Forms/Anyline/Anyline.iOS/Anyline.iOS.csproj package Anyline.Xamarin.SDK.iOS

# ARCHIVE

bundle-release: android-sdk android-examples ios-sdk ios-examples clean-build-folders
	# -- Zips the release bundle, excluding Git files, bin, obj and vs folders --
	@rm -rf anyline-ocr-xamarin-module.zip
ifeq ($(OS),Windows_NT)
	tar.exe --exclude=*.nupkg --exclude=*.DS_Store -acf anyline-ocr-xamarin-module.zip BindingSource Examples Nuget com.anyline.xamarin.examples_* LICENSE.md README.md
	CertUtil -hashfile anyline-ocr-xamarin-module.zip MD5
else
	zip -rq anyline-ocr-xamarin-module.zip . -x "*.git*" -x "*.nupkg" -x "*.DS_Store"
	md5 anyline-ocr-xamarin-module.zip
endif


# CLEAN

clean-build-folders: 
	@rm -rf BindingSource/.vs
	@rm -rf Examples/.vs

	@rm -rf Examples/Xamarin.Forms/Anyline/Anyline/bin
	@rm -rf Examples/Xamarin.Forms/Anyline/Anyline/obj

	@rm -rf Examples/AnylineExamples.Droid/bin
	@rm -rf Examples/AnylineExamples.Droid/obj

	@rm -rf Examples/AnylineExamples.iOS/bin
	@rm -rf Examples/AnylineExamples.iOS/obj

	@rm -rf BindingSource/AnylineXamarinSDK.Droid/bin
	@rm -rf BindingSource/AnylineXamarinSDK.Droid/obj

	@rm -rf BindingSource/AnylineXamarinSDK.iOS/bin
	@rm -rf BindingSource/AnylineXamarinSDK.iOS/obj

	@rm -rf Examples/Xamarin.Forms/Anyline/Anyline.Android/bin
	@rm -rf Examples/Xamarin.Forms/Anyline/Anyline.Android/obj

	@rm -rf Examples/Xamarin.Forms/Anyline/Anyline.iOS/bin
	@rm -rf Examples/Xamarin.Forms/Anyline/Anyline.iOS/obj

	@rm -rf Examples/Xamarin.Forms/Anyline/Anyline.UWP/bin
	@rm -rf Examples/Xamarin.Forms/Anyline/Anyline.UWP/obj

	@echo Build folders are clean