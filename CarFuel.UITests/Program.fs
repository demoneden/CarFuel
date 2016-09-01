//these are similar to C# using statements
open canopy
open runner
open System

//start an instance of the firefox browser
chromeDir <- "C:\chromedriver"
start chrome

let baseUrl = "http://localhost:2110" 
let userEmail = "user" + DateTime.Now.Ticks.ToString() + "@company.com"
let pwd = "Test999/*"


"Sign Up" &&& fun _ ->
    url (baseUrl + "/Account/Register")
    "#Email" << userEmail
    "#Password" << pwd
    "#ConfirmPassword" << pwd
    click "input[type=submit]"
    on baseUrl

"Log in" &&& fun _ ->
    url (baseUrl + "/Account/Login")
    "#Email" << userEmail
    "#Password" << pwd
    click "input[type=submit]"
    on baseUrl
     

"Click add link then go to create page" &&& fun _ ->
    url (baseUrl + "/Car")
    displayed "a[href='/Car/AddCar']"
    click "a[href='/Car/AddCar']"
    on (baseUrl + "/Car/AddCar")

"Create car" &&& fun _ ->
    url (baseUrl + "/Car/AddCar")
    "#Name" << "Nisson"
    click "button[type=submit]"
    on (baseUrl)

//run all tests
run()

printfn "press [enter] to exit"
System.Console.ReadLine() |> ignore

quit()