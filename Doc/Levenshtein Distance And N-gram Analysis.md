## **Context**
---

| Initial Context                                                           |    Value     | Note                                              |
| ------------------------------------------------------------------------- | :----------: | ------------------------------------------------- |
| Number of active users using the system                                   |    1 - 50    |                                                   |
| Expected number of employees for each user                                | 300 - 10,000 |                                                   |
| Average characters in Arabic names to the fourth                          |    19.45     | Based on 4000+ name,<br>Spaces between is counted |
| Average name characters without spaces "full name is between 3 - 4 names" |      17      |                                                   |
| the Average length of single name                                         |      4       |                                                   |

## **Options**


### **Levenshtein Distance**
---
#### Time complexity
**O(m\*n)** 
- `m` = length of the first string
- `n` = length of the second string

Expected time unit to search per user for maximum employees number

first name \* first matching name \* 4 names
4.25\*4.25\*4

= 72.25 t/unit 

for 10,000 records: 72.25 \* 10,000 
= 722,500 t/unit

#### Number of Operations
**m\*n\*4**
- `m` = length of the first string
- `n` = length of the second string
- `4` ≈ 3 comparisons + 1 addition
	- Deletion
	- Insertion
	- Substitution
	- Addition

#### Space needed

**Database Additional Space**
0 bytes


### **Tokens for each name: *N-gram matching***
---

#### Time complexity
**O(t \* r)** 
- `t` = number of tokens to search
- `r` = number of matching results

Worst case scenario in this is all the records have same tokens witch is almost impossible
and in this scenario average name tokens if 2 gram token is 8.
then final records lookup will be 
$$
12*10,000 = 120,000 t/unit
$$

but still this is the worst case, it's hard to determine the average without real searching data.
although real data will likely query 50% of previous number in worst case and number of tokens likely will be less if sticked to 2 grams

If used 2+3 grams the results will be more accurate but time will be 

**O((t2+t3)*r)**
- `t2` = number of 2 gram tokens to search
- `t3` = number of 3 gram tokens to search
- `r` = number of matching results

approximated 3 grams number is 8

the new result is 

$$
(8+12)*10,000 = 200,000 t/unit
$$
and still this is worst unrealistic case. and the matching 3 gram results will be significantly lower than 2 gram and the advantage of this approach is we don't do full table lookup any way

#### Number of Operations

if the n-grams is indexed, number of operations will be the number of results times counter addition.

so if didn't consider select operations and counter additions as it's done any way in both cases
the operations and time could be considered constant in this approach
#### Space needed

**Database Additional Space**
the way of saving n-gram can differ a lot in space needed, but we'll calculate the most forward one witch is storing letters as it's.

first thing we need to calculate is number of n-grams for your system based on the average name length witch is 4 it will have 
`3 from 2-gram`
`2 from 3-gram`

so 5 tokens for each single name and full name is from 4 names and each will be calculated in separate because there space needed to store them is different

**2-gram tokens**
$$
3*4*10,000=120,000Token
$$
**3-gram tokens**
$$
2*4*10,000=80,000Token
$$

SQL Server uses `UTF-16` for `VARCHAR`, and Arabic letters is in it's range so **2 bytes** only needed for the single character. 

**4 bytes** for 2-gram token
**6 bytes** for 3-gram token
foreign key of the related record in other table **4 bytes *INT***
UserId for **4 bytes *INT*** + index **4 bytes *INT***

Single 2-gram Record size 
$$
4+4+4+4=16bytes
$$
Single 3-gram Record size 
$$
6+4+4+4=18bytes
$$

total additional size per user
$$
(120,000*16)+(80,000*18) = 3.36Megabytes
$$

**NOTE:** 10,000 records need n-gram per user is the maximum number and the norm is number of records will be between 500 to 5000 at the extreme.
