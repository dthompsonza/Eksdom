# Eksdom

Service to interface with Eskom Se Push API to automate system tasks

![alt text](https://user-images.githubusercontent.com/769513/231268845-06baf5b8-eada-4bef-933e-c20d91a4fc07.png)

## Features

### Eksdom.WindowsService

This service will monitor the EskomSePush API and attempt to shutdown the machine a few minutes before scheduled loadshedding

### Integration.Eskdom.Client

I may pull this out into its own repo+nuget later, if you want a standalone implementationt try out the client from [helloserve](https://github.com/helloserve/sepush.net)

 * Check the Status of loadshedding
 * Get API allowance (API calls remaining)
 * Get Area Information
 * Caching responses to avoid wasting API credits (default 2 hour cache time)
 * Useful response models - projecting data to provide useful functionality to consumer
 
The following API functionality has not been implemented as its not used by any downstream apps. Missing:
 1. Areas Nearby (GPS)
 2. Areas Search (Text)
 3. Topics Nearby
 
## Reference

We implement the EskomSePush API. Documentation can be found at [getpostman.com](https://documenter.getpostman.com/view/1296288/UzQuNk3E#4a9eeeb8-87c2-4088-8236-1ed3626e271d).

> Note: You will need a Licence Key from EskomSePush in order to use their API. You can get a free licence key at [gumroad.com](https://eskomsepush.gumroad.com/l/api).





