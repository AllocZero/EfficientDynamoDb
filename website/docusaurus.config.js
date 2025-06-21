const {themes} = require('prism-react-renderer');
const lightCodeTheme = {
  ...themes.vsLight,
  plain: {
    ...themes.vsLight.plain,
    backgroundColor: '#f6f8fa'
  }
};
const darkCodeTheme = themes.vsDark;

module.exports = {
  title: 'EfficientDynamoDb',
  tagline: 'High performance DynamoDB library',
  url: 'https://alloczero.github.io',
  baseUrl: '/EfficientDynamoDb/',
  trailingSlash: false,
  onBrokenLinks: 'throw',
  onBrokenMarkdownLinks: 'warn',
  favicon: 'img/favicon.ico',
  organizationName: 'AllocZero', // Usually your GitHub org/user name.
  projectName: 'EfficientDynamoDb', // Usually your repo name.
  themeConfig: {
    announcementBar: {
      id: 'support_us', // Any value that will identify this message.
      content:
          '⭐️If you like EfficientDynamoDb, give it a star on <a target="_blank" rel="noopener noreferrer" href="https://github.com/AllocZero/EfficientDynamoDb">GitHub</a>! ⭐️',
      backgroundColor: '#fafbfc', // Defaults to `#fff`.
      textColor: '#091E42', // Defaults to `#000`.
      isCloseable: true, // Defaults to `true`.
    },
    navbar: {
      title: 'EfficientDynamoDb',
      logo: {
        alt: 'My Site Logo',
        src: 'img/logo.svg',
      },
      items: [
        {
          to: 'docs/',
          activeBasePath: 'docs',
          label: 'Docs',
          position: 'left',
        },
        {to: 'blog', label: 'Blog', position: 'left'},
        {
          href: 'https://github.com/AllocZero/EfficientDynamoDb',
          label: 'GitHub',
          position: 'right',
        },
      ],
    },
    footer: {
      style: 'dark',
      links: [
        {
          title: 'Docs',
          items: [
            {
              label: 'Introduction',
              to: 'docs/',
            },
            {
              label: 'Design Principles',
              to: 'docs/design/',
            },
          ],
        },
        {
          title: 'Community',
          items: [
            // {
            //   label: 'Stack Overflow',
            //   href: 'https://stackoverflow.com/questions/tagged/docusaurus',
            // },
            // {
            //   label: 'Discord',
            //   href: 'https://discordapp.com/invite/docusaurus',
            // },
            // {
            //   label: 'Twitter',
            //   href: 'https://twitter.com/docusaurus',
            // },
            {
              label: 'GitHub',
              href: 'https://github.com/AllocZero/EfficientDynamoDb',
            },
          ],
        },
        {
          title: 'More',
          items: [
            {
              label: 'Blog',
              to: 'blog',
            },
            {
              label: 'NuGet',
              href: 'https://www.nuget.org/profiles/AllocZero',
            }
          ],
        },
      ],
      copyright: `Copyright © ${new Date().getFullYear()} AllocZero`,
    },
    algolia: {
      appId: 'P7B8MW63HA',
      apiKey: '40c3e8997081db341025c9e737d05e7f',
      indexName: 'EfficientDynamoDb'
    },
    prism: {
      theme: lightCodeTheme,
      darkTheme: darkCodeTheme,
      additionalLanguages: ['csharp'],
    },
  },
  presets: [
    [
      '@docusaurus/preset-classic',
      {
        docs: {
          sidebarPath: require.resolve('./sidebars.js'),
          // Please change this to your repo.
          editUrl:
            'https://github.com/alloczero/EfficientDynamoDb/edit/main/website/',
        },
        blog: {
          showReadingTime: true,
          // Please change this to your repo.
          editUrl:
            'https://github.com/alloczero/EfficientDynamoDb/edit/main/website/blog/',
        },
        theme: {
          customCss: require.resolve('./src/css/custom.css'),
        },
      },
    ],
  ],
};
