module.exports = {
  someSidebar: {
    'Overview': [
      'efficient_dynamo_db/introduction', 
      'efficient_dynamo_db/design'
    ],
    'Developer Guide': [
      'dev_guide/setup',
      {
        'High-Level API': [
          'dev_guide/high_level/attributes',
          'dev_guide/high_level/read',
          'dev_guide/high_level/write',
          'dev_guide/high_level/batch',
          'dev_guide/high_level/transact'
        ]        
      },
      'dev_guide/retry-strategies'
    ],
    'API Reference': [
      'api_reference/region-endpoint',
      'api_reference/get-item',
      'api_reference/query',
      'api_reference/scan',
    ]
  },
};
