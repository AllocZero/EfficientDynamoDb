"use strict";(self.webpackChunkwebsite=self.webpackChunkwebsite||[]).push([[8666],{3905:(e,t,n)=>{n.d(t,{Zo:()=>p,kt:()=>g});var i=n(7294);function o(e,t,n){return t in e?Object.defineProperty(e,t,{value:n,enumerable:!0,configurable:!0,writable:!0}):e[t]=n,e}function r(e,t){var n=Object.keys(e);if(Object.getOwnPropertySymbols){var i=Object.getOwnPropertySymbols(e);t&&(i=i.filter((function(t){return Object.getOwnPropertyDescriptor(e,t).enumerable}))),n.push.apply(n,i)}return n}function a(e){for(var t=1;t<arguments.length;t++){var n=null!=arguments[t]?arguments[t]:{};t%2?r(Object(n),!0).forEach((function(t){o(e,t,n[t])})):Object.getOwnPropertyDescriptors?Object.defineProperties(e,Object.getOwnPropertyDescriptors(n)):r(Object(n)).forEach((function(t){Object.defineProperty(e,t,Object.getOwnPropertyDescriptor(n,t))}))}return e}function l(e,t){if(null==e)return{};var n,i,o=function(e,t){if(null==e)return{};var n,i,o={},r=Object.keys(e);for(i=0;i<r.length;i++)n=r[i],t.indexOf(n)>=0||(o[n]=e[n]);return o}(e,t);if(Object.getOwnPropertySymbols){var r=Object.getOwnPropertySymbols(e);for(i=0;i<r.length;i++)n=r[i],t.indexOf(n)>=0||Object.prototype.propertyIsEnumerable.call(e,n)&&(o[n]=e[n])}return o}var s=i.createContext({}),d=function(e){var t=i.useContext(s),n=t;return e&&(n="function"==typeof e?e(t):a(a({},t),e)),n},p=function(e){var t=d(e.components);return i.createElement(s.Provider,{value:t},e.children)},c="mdxType",u={inlineCode:"code",wrapper:function(e){var t=e.children;return i.createElement(i.Fragment,{},t)}},m=i.forwardRef((function(e,t){var n=e.components,o=e.mdxType,r=e.originalType,s=e.parentName,p=l(e,["components","mdxType","originalType","parentName"]),c=d(n),m=o,g=c["".concat(s,".").concat(m)]||c[m]||u[m]||r;return n?i.createElement(g,a(a({ref:t},p),{},{components:n})):i.createElement(g,a({ref:t},p))}));function g(e,t){var n=arguments,o=t&&t.mdxType;if("string"==typeof e||o){var r=n.length,a=new Array(r);a[0]=m;var l={};for(var s in t)hasOwnProperty.call(t,s)&&(l[s]=t[s]);l.originalType=e,l[c]="string"==typeof e?e:o,a[1]=l;for(var d=2;d<r;d++)a[d]=n[d];return i.createElement.apply(null,a)}return i.createElement.apply(null,n)}m.displayName="MDXCreateElement"},9574:(e,t,n)=>{n.r(t),n.d(t,{assets:()=>s,contentTitle:()=>a,default:()=>u,frontMatter:()=>r,metadata:()=>l,toc:()=>d});var i=n(7462),o=(n(7294),n(3905));const r={id:"conditions",title:"Building Conditions",slug:"../../dev-guide/high-level/conditions"},a=void 0,l={unversionedId:"dev_guide/high_level/conditions",id:"dev_guide/high_level/conditions",title:"Building Conditions",description:"This guide focuses on EfficientDynamoDb's API for building conditions.",source:"@site/docs/dev_guide/high_level/conditions.md",sourceDirName:"dev_guide/high_level",slug:"/dev-guide/high-level/conditions",permalink:"/EfficientDynamoDb/docs/dev-guide/high-level/conditions",draft:!1,editUrl:"https://github.com/alloczero/EfficientDynamoDb/edit/main/website/docs/dev_guide/high_level/conditions.md",tags:[],version:"current",frontMatter:{id:"conditions",title:"Building Conditions",slug:"../../dev-guide/high-level/conditions"},sidebar:"someSidebar",previous:{title:"Converters",permalink:"/EfficientDynamoDb/docs/dev-guide/high-level/converters"},next:{title:"Building Update Expressions",permalink:"/EfficientDynamoDb/docs/dev-guide/high-level/update-expression"}},s={},d=[{value:"Overview",id:"overview",level:2},{value:"Getting started",id:"getting-started",level:2},{value:"Conditions on array elements",id:"conditions-on-array-elements",level:3},{value:"Nested attributes",id:"nested-attributes",level:3},{value:"Comparison with other attributes",id:"comparison-with-other-attributes",level:3},{value:"Multiple conditions on a single entity",id:"multiple-conditions-on-a-single-entity",level:3},{value:"Joining multiple conditions",id:"joining-multiple-conditions",level:2},{value:"Joiner API",id:"joiner-api",level:3},{value:"Logical operators API",id:"logical-operators-api",level:3},{value:"Using both APIs together",id:"using-both-apis-together",level:3}],p={toc:d},c="wrapper";function u(e){let{components:t,...n}=e;return(0,o.kt)(c,(0,i.Z)({},p,n,{components:t,mdxType:"MDXLayout"}),(0,o.kt)("p",null,"This guide focuses on EfficientDynamoDb's API for building conditions.\nIt's assumed that you are already familiar with condition expressions in DynamoDB.\nIf not, please check out ",(0,o.kt)("a",{parentName:"p",href:"https://docs.aws.amazon.com/amazondynamodb/latest/developerguide/Expressions.ConditionExpressions.html"},"official AWS docs")," and ",(0,o.kt)("a",{parentName:"p",href:"https://docs.aws.amazon.com/amazondynamodb/latest/developerguide/Expressions.OperatorsAndFunctions.html"},"comparison operators reference")," for a better understanding of topics covered in this section."),(0,o.kt)("h2",{id:"overview"},"Overview"),(0,o.kt)("p",null,"EfficientDynamoDb aims to simplify condition expression building by providing an abstraction over DynamoDB expressions syntax."),(0,o.kt)("p",null,"Benefits of our API:"),(0,o.kt)("ul",null,(0,o.kt)("li",{parentName:"ul"},"Removes the requirement of managing ",(0,o.kt)("inlineCode",{parentName:"li"},"ExpressionAttributeNames"),", ",(0,o.kt)("inlineCode",{parentName:"li"},"ExpressionAttributeValues"),", and handling reserved words."),(0,o.kt)("li",{parentName:"ul"},"Easy refactoring and usage search in your favorite IDE.")),(0,o.kt)("h2",{id:"getting-started"},"Getting started"),(0,o.kt)("p",null,"The simplest way of creating a condition is using the ",(0,o.kt)("inlineCode",{parentName:"p"},"Condition<T>.On(...)")," static factory method:"),(0,o.kt)("pre",null,(0,o.kt)("code",{parentName:"pre",className:"language-csharp"},"var condition = Condition<EntityClass>.On(x => x.YourProperty).EqualTo(10);\n")),(0,o.kt)("p",null,(0,o.kt)("inlineCode",{parentName:"p"},"On(...)")," accepts an expression that should point to a property marked by ",(0,o.kt)("inlineCode",{parentName:"p"},"DynamoDbProperty")," attribute, element inside the collection, or the nested property of another object."),(0,o.kt)("h3",{id:"conditions-on-array-elements"},"Conditions on array elements"),(0,o.kt)("p",null,"Condition for the specific element inside the collection:"),(0,o.kt)("pre",null,(0,o.kt)("code",{parentName:"pre",className:"language-csharp"},"var condition = Condition<EntityClass>.On(x => x.YourList[3]).EqualTo(10);\n")),(0,o.kt)("p",null,"Currently, you can only use number literals, constants, fields, or variables inside the indexer.\nYou can't use methods or properties to get the index."),(0,o.kt)("p",null,"If you need to get an index from the method, you can save it to a local variable first:"),(0,o.kt)("pre",null,(0,o.kt)("code",{parentName:"pre",className:"language-csharp"},"// Correct\nvar index = GetIndex();\nvar condition = Condition<EntityClass>.On(x => x.YourList[index]).EqualTo(10);\n\n// Incorrect\nvar condition = Condition<EntityClass>.On(x => x.YourList[GetIndex()]).EqualTo(10);\n")),(0,o.kt)("h3",{id:"nested-attributes"},"Nested attributes"),(0,o.kt)("p",null,"You may access the nested attributes of lists and objects.\nE.g., the following condition is valid:"),(0,o.kt)("pre",null,(0,o.kt)("code",{parentName:"pre",className:"language-csharp"},"var condition = Condition<EntityClass>.On(x => x.TopLvlProperty.NestedList[3].MoreNestedProperty).EqualTo(10);\n")),(0,o.kt)("h3",{id:"comparison-with-other-attributes"},"Comparison with other attributes"),(0,o.kt)("p",null,"The majority of DynamoDB condition operations support comparison with other attributes instead of an explicit value.\nYou can pass an expression inside the operation method in the same way you do in ",(0,o.kt)("inlineCode",{parentName:"p"},"On(...)"),":"),(0,o.kt)("pre",null,(0,o.kt)("code",{parentName:"pre",className:"language-csharp"},"var condition = Condition<EntityClass>.On(x => x.SomeProperty).EqualTo(x => x.AnotherProperty);\n")),(0,o.kt)("p",null,"Some operations like ",(0,o.kt)("inlineCode",{parentName:"p"},"Between")," can even accept a combination of explicit values and attributes:"),(0,o.kt)("pre",null,(0,o.kt)("code",{parentName:"pre",className:"language-csharp"},"var condition = Condition<EntityClass>.On(x => x.SomeProperty).EqualTo(minValueVariable, x => x.MaxValueProperty);\n")),(0,o.kt)("h3",{id:"multiple-conditions-on-a-single-entity"},"Multiple conditions on a single entity"),(0,o.kt)("p",null,"Often, you need to create multiple conditions on a single entity.\nIn this case, the alternative API may be handy:"),(0,o.kt)("pre",null,(0,o.kt)("code",{parentName:"pre",className:"language-csharp"},"var filter = Condition.ForEntity<EntityClass>();\n\nvar firstCondition = filter.On(x => x.SomeProperty).EqualTo(10);\nvar secondCondition = filter.On(x => x.RareProperty).Exists();\n")),(0,o.kt)("p",null,"You can use these conditions in separate requests or join them into a single condition which is explained in the following section."),(0,o.kt)("h2",{id:"joining-multiple-conditions"},"Joining multiple conditions"),(0,o.kt)("p",null,"There are two ways of combining multiple conditions into one expression."),(0,o.kt)("h3",{id:"joiner-api"},"Joiner API"),(0,o.kt)("p",null,"Use any combination of ",(0,o.kt)("inlineCode",{parentName:"p"},"Joiner.And(...)")," and ",(0,o.kt)("inlineCode",{parentName:"p"},"Joiner.Or(...)")," methods to create a complex condition."),(0,o.kt)("p",null,"For example, the DynamoDB condition ",(0,o.kt)("inlineCode",{parentName:"p"},"#firstName = :firstName AND (#age < :lowerAgeLimit OR #age > :upperAgeLimit) AND begins_with(#lastName, :lastNamePrefix)")," would look like this:"),(0,o.kt)("pre",null,(0,o.kt)("code",{parentName:"pre",className:"language-csharp"},"var filter = Condition.ForEntity<EntityClass>();\nvar condition = Joiner.And(\n        filter.On(x => x.FirstName).EqualTo(firstNameValue),\n        Joiner.Or(\n            filter.On(x => x.Age).LessThan(lowerAgeLimit),\n            filter.On(x => x.Age).GreaterThan(upperAgeLimit)\n        ),\n        filter.On(x => x.LastName).BeginsWith(lastNamePrefix)\n    );\n")),(0,o.kt)("h3",{id:"logical-operators-api"},"Logical operators API"),(0,o.kt)("p",null,"You might find the Joiner API quite verbose and difficult to read when there are many ",(0,o.kt)("inlineCode",{parentName:"p"},"AND"),"/",(0,o.kt)("inlineCode",{parentName:"p"},"OR")," operators.\nThat's where logical operators come to the rescue.\nConditions in EfficientDynamoDb support logical ",(0,o.kt)("inlineCode",{parentName:"p"},"&")," and ",(0,o.kt)("inlineCode",{parentName:"p"},"|")," for combining multiple into one."),(0,o.kt)("p",null,"The same DDB expression from the Joiner API example looks like this:"),(0,o.kt)("pre",null,(0,o.kt)("code",{parentName:"pre",className:"language-csharp"},"var filter = Condition.ForEntity<EntityClass>();\nvar condition = filter.On(x => x.FirstName).EqualTo(firstNameValue) \n    & (filter.On(x => x.Age).LessThan(lowerAgeLimit) | filter.On(x => x.Age).GreaterThan(upperAgeLimit)) \n    & filter.On(x => x.LastName).BeginsWith(lastNamePrefix)\n")),(0,o.kt)("p",null,"Note that this API follows all logical operator rules, e.g., you can use parentheses to change execution order."),(0,o.kt)("h3",{id:"using-both-apis-together"},"Using both APIs together"),(0,o.kt)("p",null,"It's possible to use both Joiner and Logical operators API together to build a single query."),(0,o.kt)("pre",null,(0,o.kt)("code",{parentName:"pre",className:"language-csharp"},"var filter = Condition.ForEntity<EntityClass>();\nvar condition = Joiner.And(\n        filter.On(x => x.FirstName).EqualTo(firstNameValue),\n        filter.On(x => x.Age).LessThan(lowerAgeLimit) | filter.On(x => x.Age).GreaterThan(upperAgeLimit),\n        filter.On(x => x.LastName).BeginsWith(lastNamePrefix)\n    );\n")))}u.isMDXComponent=!0}}]);