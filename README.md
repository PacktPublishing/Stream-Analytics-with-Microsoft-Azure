## $5 Tech Unlocked 2021!
[Buy and download this Book for only $5 on PacktPub.com](https://www.packtpub.com/product/stream-analytics-with-microsoft-azure/9781788395908)
-----
*If you have read this book, please leave a review on [Amazon.com](https://www.amazon.com/gp/product/1788395905).     Potential readers can then use your unbiased opinion to help them make purchase decisions. Thank you. The $5 campaign         runs from __December 15th 2020__ to __January 13th 2021.__*

# Stream Analytics with Microsoft Azure
This is the code repository for [Stream Analytics with Microsoft Azure](https://www.packtpub.com/big-data-and-business-intelligence/stream-analytics-microsoft-azure?utm_source=github&utm_medium=repository&utm_campaign=9781788395908), published by [Packt](https://www.packtpub.com/?utm_source=github). It contains all the supporting project files necessary to work through the book from start to finish.
## About the Book
Microsoft Azure is a very popular cloud computing service used by many organizations around the world. Its latest analytics offering, Stream Analytics, allows you to process and get actionable insights from different kinds of data in real-time.

This book is your guide to understanding the basics of how Azure Stream Analytics works, and building your own analytics solution using its capabilities. You will start with understanding what Stream Analytics is, and why it is a popular choice for getting real-time insights from data. Then, you will be introduced to Azure Stream Analytics, and see how you can use the tools and functions in Azure to develop your own Streaming Analytics. Over the course of the book, you will be given comparative analytic guidance on using Azure Streaming with other Microsoft Data Platform resources such as Big Data Lambda Architecture integration for real time data analysis and differences of scenarios for architecture designing with Azure HDInsight Hadoop clusters with Storm or Stream Analytics. The book also shows you how you can manage, monitor, and scale your solution for optimal performance.

By the end of this book, you will be well-versed in using Azure Stream Analytics to develop an efficient analytics solution that can work with any type of data.

## Instructions and Navigation
All of the code is organized into folders. Each folder starts with a number followed by the application name. For example, Chapter02.



The code will look like the following:
```
Select input.vin, BlobSource.Model, input.timestamp,
input.outsideTemperature,
input.engineTemperature, input.speed, input.fuel, input.engineoil,
input.tirepressure, input.odometer, input.city,
input.accelerator_pedal_position,
input.parking_brake_status,
input.headlamp_status, input.brake_pedal_status,
input.transmission_gear_position, input.ignition_status,
input.windshield_wiper_status, input.abs into output from input join
BlobSource
on input.vin = BlobSource.VI
```

1. A valid Azure subscription
2. Visual Studio 2017/2015
3. Azure SDK 2.7.1 or higher
4. Azure Storage Explorer
5. A PowerBI Office 365 account
6. Python SDK 2.7 (x64) bit and packages

## Related Products
* [Mastering Microsoft Power BI](https://www.packtpub.com/big-data-and-business-intelligence/mastering-microsoft-power-bi?utm_source=github&utm_medium=repository&utm_campaign=9781788297233)

* [Internet of Things with Raspberry Pi 3](https://www.packtpub.com/virtualization-and-cloud/internet-things-raspberry-pi-3?utm_source=github&utm_medium=repository&utm_campaign=9781788627405)

* [Practical Big Data Analytics](https://www.packtpub.com/big-data-and-business-intelligence/practical-big-data-analytics?utm_source=github&utm_medium=repository&utm_campaign=9781783554393)

### Suggestions and Feedback
[Click here](https://docs.google.com/forms/d/e/1FAIpQLSe5qwunkGf6PUvzPirPDtuy1Du5Rlzew23UBp2S-P3wB-GcwQ/viewform) if you have any feedback or suggestions.
