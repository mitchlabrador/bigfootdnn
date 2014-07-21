BigfootDNN
==========

**MVC Framework for DNN (DotNetNuke)**

**A bit of history...**
A while back I wanted to create a way to do MVC style development in ASP.NET WebForms to this end I created BigfootMVC (http://bigfootmvc.codeplex.com). BigfootMVC was later expanded to allow MVC development on the DNN platform. The end result was a hybrid framework that allowed me to execute MVC style rendering as well as dynamic rendering of WebForms controls. This worked fine and was used in several production environments including a commercial module. I was never quite happy with it however as I wanted something that was a bit more specific to MVC style development in DNN. BigfootDNN is my effort at making BigfootMVC a focused MVC framework for DNN.

This is definitely a work in progress an requires a ton of testing including a few solid refactoring passes, but it creates a good foundation for the work to begin. I will be creating a sample project to showcase how to best utilize the framework as I develop it into my new commercial module for DNN.

**A few highlights...**
 - Rendering is done using the Razor view engine
 - A full and extensive set of helper methods for Html / JavaScript / Ajax patterns
 - An accompanying **bigfoot.js** javascript library to provide the JS functionality for the helpers
 - A solid set of utlity CSS classes for rapid HTML prototyping **bigfoot.css**
 - An extensive application object that understands DNN and knows how to interact with its various services properly (Email, Logging, Caching, Storage, etc.)
 - **BigfootSql** a MicroORM which understands DNN. For example {{TableName}} will properly translate into dbowner.dnnprefix_moduleprefix_tablename when executing. It is a StringBuilder for SQL on steroids with object hydration capabilities and DNN naming expansions built in

**Requirements:**
 - .NET 4.0 and above
 - DNN 6.0.1 and above
 - jQuery (used by the helpers extensively)

**Finally...** This effort also flows from a lot of the great open source code around the web which has served to inspire this effort. Very little code is new anymore, more often than not our code is a recycling of old ideas implemented in slightly different ways for specific contexts. HostRazor module, DNN Community engine, etc. have been but a few of the projects that have inspired a lot of this effort.
