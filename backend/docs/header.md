- - -

## Access token

For all request an access token is required.  
In `GET` requests the access token is sent in the query string.

**Example:**
```
?access_token=xxx.yyy.zzz
```

In `POST`, `PUT` and `DELETE` requests the access token is sent in the request body.

**Example:**
```
Content-Type: application/json
{
    "access_token": "xxx.yyy.zzz"
}
```

The access token can be retrieved using the [/authenticate](#api-Authentication-Authenticate) endpoint.

## Date format

All the dates used in the API are represented using ISO-8601 format in Coordinated Universal Time (UTC).

**Format:** `YYYY-MM-DDThh:mm:ss.fffZ`  
**Example:** `2015-15-03T15:30:00.000Z`  

All the dates used in request bodies and query strings should be in this format. Make sure to do the appropriate conversion between Local Time and UTC.

## Query strings

Most `GET` request allows filtering, sorting, paging and field selection. This is achieved using query strings.

### Filtering

The parameter `q` is used to only retrieve those objects that satisfies one or more conditions.

**Format:** `?q=condition,condition,condition,...`  
**Condition:** `field+operator+value`

The supported operators are:

- `eq`: Equals.
- `ne`: Not Equals.
- `ge`: Greater Equals.
- `le`: Lower Equals.
- `lt`: Lower Than.
- `gt`: Greater Than.
- `like`: Like. [Wildcard: `*`]

**Examples:**

Filter students by gender. Retrieve only male students.  
```
GET /students?q=gender+eq+masculino HTTP/1.1
```

Filter courses by code. Retrieve only the courses whose code starts with CI-.
```
GET /courses?q=code+like+CI-* HTTP/1.1
```

Filter appointments by date. Retrieve only the appointments made in the year 2015.
```
GET /appointments?q=date+ge+2015-01-01T00:00:00.000Z,date+lt+2016-01-01T00:00:00.000Z HTTP/1.1
```

### Sorting

The parameter `sort` is used to sort the results by one or more fields. Each field can be sorted in ascending or descending order.

**Format:** `?sort=[-]field,[-]field,[-]field,...`

**Examples:**

Sort professor by ascending username.
```
GET /professors?sort=username HTTP/1.1
```

Sort software by ascending code and then by descending name.
```
GET /software?sort=code,-name HTTP/1.1
```

Sort operators by descending period year.
```
GET /operators?sort=-period.year HTTP/1.1
```

### Paging

The parameters `page` and `limit` are used to paginate the results. The default values for this parameters are `page=1` and `limit=20`.  
Use `limit=-1` to retrieve all objects.

**Format:** `?page=n` `?limit=m`

**Examples:**

Retrieve the first 30 operators.
```
GET /operators?limit=30 HTTP/1.1
```

Retrieve the next 30 operators.
```
GET /operators?page=2&limit=30 HTTP/1.1
```

Retrieve all the laboratories.
```
GET /laboratories?limit=-1 HTTP/1.1
```

### Field selection

The parameter `fields` is used to include only certain object attributes in the results.

**Format:** `?fields=field,field,field,...`

**Example:**

Retrieve only the start_date and end_date in reservations:
```
GET /reservations?fields=start_date,end_date HTTP/1.1
```

Retrieve only the id, course code and professor username in groups.
```
GET /groups?fields=id,course.code,professor.username HTTP/1.1
```
