
# This is the HackerNewsApi

Project is a  API which returns the last news from https://news.ycombinator.com/

## How to run the app
    - mkdir hackernewsapi
    - git clone https://github.com/ducatispeed/HackerNewsApi.git
    

## How to run the unit tests
    - dotnet test

## How to get the data
    - curl GET 'http://localhost:5000/hackernews'

## Cache
The default request use in memory cache to avoid get again the same stories, to disable it use:
    - curl GET 'http://localhost:5000/hackernews' --header 'DisableCache: true'

For cache cleaning
    - curl GET 'http://localhost:5000/clean'
