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
        type: 'category',
        label: 'Request Builders',
        link: {
          type: 'generated-index',
          title: 'Request Builders',
          slug: 'api-reference/builders',
        },
        items: [
          'api_reference/builders/get-item-builder',
          'api_reference/builders/put-item-builder',
          'api_reference/builders/delete-item-builder',
          'api_reference/builders/update-item-builder',
          'api_reference/builders/query-builder',
          'api_reference/builders/scan-builder',
        ],
      },
      {
        type: 'category',
        label: 'Request Options',
        link: {
          type: 'generated-index',
          title: 'Request Options',
          slug: `api-reference/options`,
        },
        items: [
          'api_reference/options/select-mode',
          'api_reference/options/return-values',
        ],
      }
    ],
  },
};
