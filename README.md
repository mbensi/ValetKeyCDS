# Valet Key Pattern

This document describes the Valet Key Pattern example from the guide [Cloud Design Patterns](http://aka.ms/Cloud-Design-Patterns).

This version has been modified to use a Web API instead of the Cloud Service in the original sample. It also contains a DownloadBlob method in the client, to retrieve the uploaded blob from the Azure Storage, hence implementing the actual Valet Key pattern.

## System Requirements

* Microsoft .NET Framework version 4.5
* Microsoft Visual Studio 2015 Comunity, Enterprise, or Professional
* Windows Azure SDK for .NET version 2.9

## Before you start

Ensure that you have installed all of the software prerequisites.

## About the Example
 
This example shows how a client application can obtain a shared access signature with the necessary permissions to write directly to blob storage. For simplicity, this sample focuses on the mechanism to obtain and consume a valet key and does not show how to implement authentication or secure communications.

## Running the Example

You can run this example locally in the Visual Studio Windows Azure emulator. You can also run this example by deploying it to a Windows Azure App Service.

* If you want to run the example on Windows Azure, provision a Windows Azure App Service and deploy the application to it from Visual Studio. Alternatively you can run the example in the local IIS Express.

* Start the Web Service and note the URL of the web API shown in the browser address bar.
* Open the file app.config from the ValetKey.Client project and change the setting for serviceEndpointUrl to   [your-URL]**/api/valetkey/**
	* By default this is set to **http://localhost:11549/api/valetkey/**


* Start a new instance of the ValetKey.Client project to upload the blob. Right-click the project, select Debug, and click Start new instance.



