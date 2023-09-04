module.exports = {
  someSidebar: {
    'Overview': [
      'efficient_dynamo_db/introduction', 
      'efficient_dynamo_db/design'
    ],
    'Developer Guide': [
      'dev_guide/getting-started',
      {
        'Configuration': [
          'dev_guide/configuration/region-endpoint',
          'dev_guide/configuration/credentials',
          'dev_guide/configuration/retry-strategies',
        ]
      },
      {
        'High-Level API': [
          'dev_guide/high_level/attributes',
          'dev_guide/high_level/read',
          'dev_guide/high_level/write',
          'dev_guide/high_level/batch',
          'dev_guide/high_level/transact',
          'dev_guide/high_level/converters',
          'dev_guide/high_level/conditions',
          'dev_guide/high_level/update-expression',
        ],
      },
      'dev_guide/low-level',
      'dev_guide/sdk-compatibility',
    ],
    'API Reference': [
      {
        'Request Builders': [
          'api_reference/builders/get-item-builder',
          'api_reference/builders/query-builder'
        ],
      },
      'api_reference/scan',
      'api_reference/put-item',
      'api_reference/update-item',
      'api_reference/delete-item',
    ],
  },
};
