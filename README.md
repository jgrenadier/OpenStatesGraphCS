# OpenStatesGraphCS
OpenStatesGraphCS consists of several projects:

Firstly, a .Net library for accessing the new Open States GraphQL API.
OpenStates ( https://openstates.org/ )provides legislative data at the U.S state level 
and GraphQL is a popular query language first developed at Facebook. GraphQL
( https://graphql.org/ )is particularly good at allowing you to query 
for just the subsets of data that you need.

In addition of the .Net library, we have a included a Windows standalone application
to exercise the library.

Finally, OpenStatesGraphCS includes a Microsoft Azure Bot to allow end users to
query for OpenStates data.  The virtue of Microsoft Bots that they can be hosted 
from many converstational platforms such Slack, Skype, Facebook Messenger without
having to change a single line of code.

For now, we have implemented the dialogs using the LUIS natural language 
engine.  This allows the user to converse with the bot in english phrases
and the our bot will do it's best to interpret the query.



