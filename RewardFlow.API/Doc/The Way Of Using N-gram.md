# How Tokens will be handled
---
Date: 10-8-2025

## Db Structure: 
---

| Couloum     | Description                                   |
| ----------- | --------------------------------------------- |
| user_id     | foreign key, indexed to user table            |
| token_hased | the n-gram token hashed for security          |
| n           | the length of the token                       |
| employee_id | foreign key not indexed referring to employee |

## how the name will be tokenized
---

by general common class called Tokenizer make the ability to tokenize a string to array of tokens from 2-gram "2 characters" or more 
and have the ability to include spaces but by default it's not included


## The flow of the app
---

**Add New**
saves the employee to the db then tokonize the name to 2-gram, 3-gram.
and the 3-gram includes spaces then saves it to database

**Update**
if the update in name delete old tokens in the table then create new ones

**Delete**
Delete the tokens

**Search**
select the tokens by user id and group by indexed tokens to employees and have a counter in this group by to count how many times the token is repeated for this employees then select top(x) 10 for now

## NOTE
---

Tokenization not in API handler, it should be a separate method then the handler call it