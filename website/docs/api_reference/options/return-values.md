---
id: return-values
title: ReturnValues
slug: ../../api-reference/options/return-values
---

`ReturnValues` enum specifies what data will be returned after the write request.

The subset of supported `ReturnValues` values is different for every write operation.
Refer to the API reference page of the operation or the AWS docs to see which values are supported in your case.

All possible values are:

- `None`: No values are returned in the response.
- `AllOld`: Returns all of the attributes of the item, as they appeared before the operation.
- `UpdatedOld`: Returns only the updated attributes, as they appeared before the operation.
- `AllNew`: Returns all of the attributes of the item, as they appear after the operation.
- `UpdatedNew`: Returns only the updated attributes, as they appear after the operation.
