---
id: introduction
title: Introduction
sidebar_label: Introduction
slug: /
---

EfficientDynamoDb is a high performance DynamoDb library with a huge focus on efficient resources utilization. Due to DynamoDb extreme scaling capabilities it is very important for backend services to not waste valuable CPU time on unmarshalling responses. EfficientDynamoDb is capable of zero allocation deserialization. 

In general it allocates up to 26X less memory and is up to 21X faster than official AWS SDK.