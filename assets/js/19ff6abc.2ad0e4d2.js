"use strict";(self.webpackChunkwebsite=self.webpackChunkwebsite||[]).push([[2120],{3905:(e,t,n)=>{n.d(t,{Zo:()=>p,kt:()=>d});var r=n(7294);function i(e,t,n){return t in e?Object.defineProperty(e,t,{value:n,enumerable:!0,configurable:!0,writable:!0}):e[t]=n,e}function a(e,t){var n=Object.keys(e);if(Object.getOwnPropertySymbols){var r=Object.getOwnPropertySymbols(e);t&&(r=r.filter((function(t){return Object.getOwnPropertyDescriptor(e,t).enumerable}))),n.push.apply(n,r)}return n}function o(e){for(var t=1;t<arguments.length;t++){var n=null!=arguments[t]?arguments[t]:{};t%2?a(Object(n),!0).forEach((function(t){i(e,t,n[t])})):Object.getOwnPropertyDescriptors?Object.defineProperties(e,Object.getOwnPropertyDescriptors(n)):a(Object(n)).forEach((function(t){Object.defineProperty(e,t,Object.getOwnPropertyDescriptor(n,t))}))}return e}function l(e,t){if(null==e)return{};var n,r,i=function(e,t){if(null==e)return{};var n,r,i={},a=Object.keys(e);for(r=0;r<a.length;r++)n=a[r],t.indexOf(n)>=0||(i[n]=e[n]);return i}(e,t);if(Object.getOwnPropertySymbols){var a=Object.getOwnPropertySymbols(e);for(r=0;r<a.length;r++)n=a[r],t.indexOf(n)>=0||Object.prototype.propertyIsEnumerable.call(e,n)&&(i[n]=e[n])}return i}var c=r.createContext({}),s=function(e){var t=r.useContext(c),n=t;return e&&(n="function"==typeof e?e(t):o(o({},t),e)),n},p=function(e){var t=s(e.components);return r.createElement(c.Provider,{value:t},e.children)},m="mdxType",u={inlineCode:"code",wrapper:function(e){var t=e.children;return r.createElement(r.Fragment,{},t)}},h=r.forwardRef((function(e,t){var n=e.components,i=e.mdxType,a=e.originalType,c=e.parentName,p=l(e,["components","mdxType","originalType","parentName"]),m=s(n),h=i,d=m["".concat(c,".").concat(h)]||m[h]||u[h]||a;return n?r.createElement(d,o(o({ref:t},p),{},{components:n})):r.createElement(d,o({ref:t},p))}));function d(e,t){var n=arguments,i=t&&t.mdxType;if("string"==typeof e||i){var a=n.length,o=new Array(a);o[0]=h;var l={};for(var c in t)hasOwnProperty.call(t,c)&&(l[c]=t[c]);l.originalType=e,l[m]="string"==typeof e?e:i,o[1]=l;for(var s=2;s<a;s++)o[s]=n[s];return r.createElement.apply(null,o)}return r.createElement.apply(null,n)}h.displayName="MDXCreateElement"},1430:(e,t,n)=>{n.r(t),n.d(t,{assets:()=>c,contentTitle:()=>o,default:()=>u,frontMatter:()=>a,metadata:()=>l,toc:()=>s});var r=n(7462),i=(n(7294),n(3905));const a={id:"batch",title:"Batch Operations",slug:"../../dev-guide/high-level/batch"},o=void 0,l={unversionedId:"dev_guide/high_level/batch",id:"dev_guide/high_level/batch",title:"Batch Operations",description:"DynamoDB provides two batch operations:",source:"@site/docs/dev_guide/high_level/batch.md",sourceDirName:"dev_guide/high_level",slug:"/dev-guide/high-level/batch",permalink:"/EfficientDynamoDb/docs/dev-guide/high-level/batch",draft:!1,editUrl:"https://github.com/alloczero/EfficientDynamoDb/edit/main/website/docs/dev_guide/high_level/batch.md",tags:[],version:"current",frontMatter:{id:"batch",title:"Batch Operations",slug:"../../dev-guide/high-level/batch"},sidebar:"someSidebar",previous:{title:"Writing Data",permalink:"/EfficientDynamoDb/docs/dev-guide/high-level/write"},next:{title:"Transactions",permalink:"/EfficientDynamoDb/docs/dev-guide/high-level/transact"}},c={},s=[{value:"BatchGetItem",id:"batchgetitem",level:2},{value:"BatchWriteItem",id:"batchwriteitem",level:2}],p={toc:s},m="wrapper";function u(e){let{components:t,...n}=e;return(0,i.kt)(m,(0,r.Z)({},p,n,{components:t,mdxType:"MDXLayout"}),(0,i.kt)("p",null,"DynamoDB provides two batch operations:"),(0,i.kt)("ul",null,(0,i.kt)("li",{parentName:"ul"},(0,i.kt)("inlineCode",{parentName:"li"},"BatchGetItem")," - Read multiple items from one or more tables."),(0,i.kt)("li",{parentName:"ul"},(0,i.kt)("inlineCode",{parentName:"li"},"BatchWriteItem")," -  Put or delete multiple items in one or more tables.")),(0,i.kt)("p",null,"EfficientDynamoDb automatically delays and retries if the batch operation returns unprocessed items,\nwhich can happen when provisioned throughput is exceeded, the size limit is reached, or an internal DynamoDB error occurred."),(0,i.kt)("h2",{id:"batchgetitem"},"BatchGetItem"),(0,i.kt)("p",null,"Reads up to 100 items in a single request."),(0,i.kt)("p",null,"Each entity's primary key is configured using ",(0,i.kt)("inlineCode",{parentName:"p"},"Batch.GetItem")," factory method."),(0,i.kt)("pre",null,(0,i.kt)("code",{parentName:"pre",className:"language-csharp"},' var items = await context.BatchGet()\n     .WithItems(\n         Batch.GetItem<EntityClass>().WithPrimaryKey("partitionKey", "sortKey_1"),\n         Batch.GetItem<EntityClass>().WithPrimaryKey("partitionKey", "sortKey_2")\n     )\n     .ToListAsync<EntityClass>();\n')),(0,i.kt)("p",null,"When strong consistency or a projection is needed, a more sophisticated ",(0,i.kt)("inlineCode",{parentName:"p"},"FromTables")," method can be used:"),(0,i.kt)("pre",null,(0,i.kt)("code",{parentName:"pre",className:"language-csharp"},' var items = await context.BatchGet()\n     .FromTables(\n        Batch.FromTable<EntityClass>()\n            .WithConsistentRead(true)\n            .WithProjectedAttributes<ProjectionClass>()\n            .WithItems(\n                Batch.GetItem<EntityClass>().WithPrimaryKey("partitionKey", "sortKey_1"),\n                Batch.GetItem<EntityClass>().WithPrimaryKey("partitionKey", "sortKey_2")\n                )\n    )\n     .ToListAsync<EntityClass>();\n')),(0,i.kt)("p",null,(0,i.kt)("em",{parentName:"p"},"Entities of different types can be retrieved by using ",(0,i.kt)("inlineCode",{parentName:"em"},"AsDocuments()")," method the same way as for other read operations.")),(0,i.kt)("h2",{id:"batchwriteitem"},"BatchWriteItem"),(0,i.kt)("p",null,"Puts or deletes up to 25 items in a single request."),(0,i.kt)("p",null,"Each write operation is configured using either ",(0,i.kt)("inlineCode",{parentName:"p"},"Batch.PutItem")," or ",(0,i.kt)("inlineCode",{parentName:"p"},"Batch.DeleteItem")," factory method."),(0,i.kt)("pre",null,(0,i.kt)("code",{parentName:"pre",className:"language-csharp"},'await context.BatchWrite()\n    .WithItems(\n        Batch.PutItem(new UserEntity("John", "Doe")),\n        Batch.DeleteItem<UserEntity>().WithPrimaryKey("partitionKey", "sortKey")\n    )\n    .ExecuteAsync();\n')))}u.isMDXComponent=!0}}]);