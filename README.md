BigfootDNN
==========

**MVC Framework for DNN (DotNetNuke)**

**A bit of history...**
A while back I wanted to create a way to do MVC style development in ASP.NET WebForms to this end I created BigfootMVC (http://bigfootmvc.codeplex.com). BigfootMVC was later expanded to allow MVC development on the DNN platform. The end result was a hybrid framework that allowed me to execute MVC style rendering as well as dynamic rendering of WebForms controls. This worked fine and was used in several production environments including a commercial module. I was never quite happy with it however as I wanted something that was a bit more specific to MVC style development in DNN. BigfootDNN is my effort at making BigfootMVC a focused MVC framework for DNN.

This is definitely a work in progress an requires a ton of testing including a few solid refactoring passes, but it creates a good foundation for the work to begin. I will be creating a sample project to showcase how to best utilize the framework as I develop it into my new commercial for DNN.

**So a few of the highlights:**
 - Rendering is done using the Razor view engine
 - A full and extensive set of helper methods for Html / JavaScript / Ajax patterns
 - An accompanying **bigfoot.js** javascript library to provide the JS functionality for the helpers
 - A solid set of utlity CSS classes for rapid HTML prototyping **bigfoot.css**
 - An extensive application object that understands DNN and knows how to interact with its various services properly (Email, Logging, Caching, Storage)
 - BigfootSql a MicroORM which understands DNN for example, {{TableName}} will properly translate in to dbowner.dnnprefix_moduleprefix_tablename. It is a StringBuilder for SQL on steroids with object hydration capabilities

**Requirements:**
 - .NET 4.0 and above
 - DNN 6.0.1 and above
