module.exports = {
  someSidebar: {
    'Overview': [
      'efficient_dynamo_db/introduction', 
      'efficient_dynamo_db/design'
    ],
    'Developer Guide': [
      {
        'Getting started': [
          'dev_guide/getting_started/getting-started',
          'dev_guide/getting_started/region-endpoint',
          'dev_guide/getting_started/credentials',
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
          'dev_guide/high_level/update-expression'
        ],
      },
      'dev_guide/low-level',
      'dev_guide/retry-strategies',
      'dev_guide/sdk-compatibility'
    ],
    'API Reference': [
      'api_reference/get-item',
      'api_reference/query',
      'api_reference/scan',
      'api_reference/put-item',
      'api_reference/update-item',
      'api_reference/delete-item'
    ]
  },
};
