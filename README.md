# Jobscraper

This tool currently scans Jobindex.dk for job ads and scrapes the page content of all ads that it finds. The result of the scraping is subsequently stored in a database. The user is then able to filter the ads that have been scraped by entering keywords that should be in the ad and thus only be shown relevant ads.

There are still more things I would like to add to this project so here is a todo.
- [X] Add must, should and cannot to keywords.
- [X] Add company name to the ads being shown.
- [ ] Replace JobIndexScraper with a generic base scraper such that a scraper for any specific site can be added by simply inputting information about what an ad looks like and how to navigate the site.

Demonstration:
![til](https://i.imgur.com/9EBTE93.gif)
