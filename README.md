# Social Media API

## Overview
This is a demo project created for learning purposes, as it is my first .NET Core application.

The API serves as the backend for a social media platform, which I plan to integrate into a website or mobile application.


## Features
#### Authenticated users can:
- Register and log in
- Create, edit, and delete posts
- Like and unlike posts
- Comment on posts, edit or delete their comments
- Send, read, and delete chat messages

#### Unauthenticated users can:
- View the list of registered users
- View the profile of a specific user
- Browse the latest posts or posts from a specific user
- View comments on posts or user profiles
- See who liked a post or what a user has liked


## Tech stack
- .NET Core 8
- SQLite database (temporary)


## Deployment
- TODO


## Usage
#### API Documentation
All endpoints are documented and accessible via /swagger.

#### Pagination
When retrieving multiple objects (lists), the API returns a paginated response.
##### By default:
- Page size: 10 (means 10 objects per page)
- Page number: 1

If pageNumber and pageSize parameters are not specified in the query, these default values are applied.