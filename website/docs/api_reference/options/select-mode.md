---
id: select-mode
title: Select Mode
slug: ../../api-reference/options/select-mode
---

Select mode specifies what data is returned during the `Query` and `Scan` operations.

The possible values are:

- `AllAttributes`: Returns all of the item attributes from the specified table or index.
If you query a local secondary index, then for each matching item in the index, DynamoDB fetches the entire item from the parent table.
If the index is configured to project all item attributes, then all of the data can be obtained from the local secondary index, and no fetching is required.
- `AllProjectedAttributes`: Allowed only when querying an index.
Retrieves all attributes that have been projected into the index.
If the index is configured to project all attributes, this return value is equivalent to specifying `AllAttributes`.
- `Count`: Returns the number of matching items, rather than the matching items themselves.
- `SpecificAttributes`: If you query or scan a local secondary index and request only attributes that are projected into that index, the operation will read only the index and not the table.
  - If any of the requested attributes are not projected into the local secondary index, DynamoDB fetches each of these attributes from the parent table.
  This extra fetching incurs additional throughput cost and latency.
  - If you query or scan a global secondary index, you can only request attributes that are projected into the index. Global secondary index queries cannot fetch attributes from the parent table.

:::tip
`SpecificAttributes` mode is equivalent to using projection methods in `Scan` and `Query` builders without specifying any value for select mode.
:::
