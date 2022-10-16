
# DATACOM Code Test

## Build
* To run the **web** project:
    1. Navigate to the `Datacom.PayslipGenerator.Web\ClientApp\` folder and run `yarn install`
    2. Navigate to the `Datacom.PayslipGenerator.Web\` folder and run `run-dev.cmd`
        * **Note** that running this for the first time will download an open-source runner that I created from NetPotential's BitBucket account. 
            It is a bootstrapper for running .NET Core projects and does not install anything.
        * Alternatively, from `Visual Studio 2022` set `Datacom.PayslipGenerator.Web` as the startup project, and press F5. (see notes below)
    3. From a browser, go to https://localhost:7017/ if it did not auto-launch.
* To run the **console** project:
    1. Select the `Datacom.PayslipGenerator.Console` project and run
        * **Note** that the console project only runs functional tests and does not accept any input

## Assumptions
* Financial year starts on Jan 1st
* The current year is 2022
* Payment period is monthly (Fortnightly has also been implemented but not used)
* Locale is en-NZ
* Default rounding is to the nearest number, when a number is halfway
* Default tax bracket is as per requirement

## Notes

* There is a build task that should compile React and SCSS on build so F5 in theory should work as long as the packages have been previously restored.
    * If after launching, the React code is not automatically compiled, run `npm run watch` or `npm run build-development` under the `ClientApp` sub directory.
* The unit tests do not cover full coverage

## Preview

To access a working preview, go to https://datacomcodetest-dealdiane.azurewebsites.net/