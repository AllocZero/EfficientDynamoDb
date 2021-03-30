import React from 'react';
import clsx from 'clsx';
import Layout from '@theme/Layout';
import Head from '@docusaurus/Head';
import Link from '@docusaurus/Link';
import useDocusaurusContext from '@docusaurus/useDocusaurusContext';
import useBaseUrl from '@docusaurus/useBaseUrl';
import styles from './styles.module.css';

const features = [
  {
    title: 'Blazing Fast',
    imageUrl: 'img/performance.svg',
    description: (
      <>
          Allocates up to 26X less memory and is up to 21X faster than official AWS SDK.
      </>
    ),
  },
  {
    title: 'Pay Less, Scale More',
    imageUrl: 'img/cost.svg',
    description: (
      <>
        Use your CPU for something meaningful, not to deserialize responses. <br/>
        You only need a quarter of current computing capacity to handle all DynamoDB workloads.
      </>
    ),
  },
  {
    title: 'The one API to rule them all',
    imageUrl: 'img/api.svg',
    description: (
      <>
          Performing batch requests, transactions, complex filters and updates has never been easier. <br/>
          Build any DynamoDb expression in pure C#. 
          
      </>
    ),
  },
];

function Feature({imageUrl, title, description}) {
  const imgUrl = useBaseUrl(imageUrl);
  return (
    <div className={clsx('col col--4', styles.feature)}>
      {imgUrl && (
        <div className="text--center">
          <img className={styles.featureImage} src={imgUrl} alt={title} />
        </div>
      )}
      <h3>{title}</h3>
      <p>{description}</p>
    </div>
  );
}

function Home() {
  const context = useDocusaurusContext();
  const {siteConfig = {}} = context;
  return (
    <Layout
      title={`${siteConfig.title}`}
      description="High performance DynamoDB library">

      <Head>
          <meta name="google-site-verification" content="11J8VjLsYwyOoX0yfCMP_9eO9VKEYr6Iwi25JAkCytI" />
      </Head>
        
      <header className={clsx('hero', styles.heroBanner)}>
        <div className={clsx("container", styles.heroContainer)}>
            <div>
                <img alt="DynamoDb" className={styles.heroLogo} src="img/dynamodb.png" height={150}/>
            </div>
            <div>
                <h1 className="hero__title">{siteConfig.title}</h1>
                <p className="hero__subtitle">{siteConfig.tagline}</p>
                <div className={styles.buttons}>
                    <Link
                        className={clsx(
                            'button button--outline button--secondary button--lg',
                            styles.getStarted,
                        )}
                        to={useBaseUrl('docs/')}>
                        Get Started
                    </Link>
                </div>
            </div>
        </div>
      </header>
      <main className={styles.efficientMain}>
          <div>
              {features && features.length > 0 && (
                  <section className={styles.features}>
                      <div className="container">
                          <div className="row">
                              {features.map((props, idx) => (
                                  <Feature key={idx} {...props} />
                              ))}
                          </div>
                      </div>
                  </section>
              )}
          </div>
          
          <div className={styles.nugetContainer}>
              <h1>Try it Now</h1>
              <div className={styles.nugetCodeBlock}>
                  <code>Install-Package <a target="_blank" rel="noopener noreferrer"
                                           href="https://www.nuget.org/packages/EfficientDynamoDb">EfficientDynamoDb</a> -Version 0.9.0</code>
              </div>
          </div>
      </main>
    </Layout>
  );
}

export default Home;
