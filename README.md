# ScraperCase
ScraperCase based on selenium

- URL of the Report.


    https://docs.google.com/spreadsheets/d/1JsRD6MQXwMyliPyZkplJ-T4t8Lur1QZEWEpnqFxL9ho/edit?usp=sharing
  
  
  
- Used tech-stack ( library/framework etc.)


    FrameWork:
        .Net Core
    
    
    NuGets:
        Google.Apis.Sheets.v4
        Selenium.Support
        Selenium.WebDriver
        Selenium.WebDriver.ChromeDriver
        Selenium.WebDriverBackedSelenium
        WebDriver.ChromeDriver.win32
    
    
    Services:
        https://developers.google.com/sheets/api
    
    
    



- A brief description of the challenges you face.
    
    
    -First challenge was I had no idea about scraping.After a few readings I understood the idea.
    -Second one was I did not use use Python and did not have much experience with JAVA because of this I use .Net Core framework.
    -Third one was caused by google. It did not allow me to create a new credential to use API.

   
   
   
   
- What did you learn from this project?
    
    1. There is still lots of thing o learn that makes me excited.
    2. I must improve my research skills to understand the problems and solve them faster.




- Answers of the additional questions.
        Additional Questions
        
        
        1. If I’d have 10.000 urls that I should visit, then it takes hours to finish. What
        can we make to fasten this process?
           -Even if we have 100 url to visit it may take around 1 hour. We can get these 10.000 data as list (from a Json file or from a text) then we can manipulate the data by parsing with the methods. (Based on my search, Python has an event-driven networking engine called “Python Twisted”. We can use this kind of libraries, packages to manipulate the meta-datas)
       2. What can we make or use to automate this process to run once a day? Write your recommendations.
          -We can set a method to visit specified urls from a file (Json, txt etc.) and this method can be set to initialize at 8.00 am.
       3. Please briefly explain what an API is and how it works.
          -API is a software definition and technology that help the communication between two applications (from web app. to web app., web app. to mobile app., mobile app to web app. Etc.). It helps to data transfer without touching the main-real data security since it is a layer between data and server.

