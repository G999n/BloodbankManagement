CRUD Operations

Create (POST /api/bloodbank): Add a new entry to the blood bank list. The input should include donor details, blood type, quantity, and collection/expiration dates.

Read (GET /api/bloodbank): Retrieve all entries in the blood bank list.

Read (GET /api/bloodbank/{id}): Retrieve a specific blood entry by its Id.

Update (PUT /api/bloodbank/{id}): Update an existing entry (e.g., modify donor details or update blood status).

Delete (DELETE /api/bloodbank/{id}): Remove an entry from the list based on its Id.

2. Pagination

GET /api/bloodbank?page={pageNumber}&size={pageSize}: Retrieve a paginated list of blood bank entries. The response should show entries based on page number and page size parameters.

3. Search Functionality
GET /api/bloodbank/search?bloodType={bloodType}: Search for blood bank entries based on blood type.

GET /api/bloodbank/search?status={status}: Search for blood bank entries by status (e.g., "Available", "Requested").

GET /api/bloodbank/search?donorName={donorName}: Search for donors by name.