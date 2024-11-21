#!/usr/bin/env bash

set -e

# Define hermes-specific topics
hermes_topics=(
  DI.ProviderFixture.Raw 
  DI.ProviderFixture.Validated 
  DI.ProviderFixture.Enriched 
  DI.ProviderFixture.MetadataMapped 
  DI.ProviderFixture.FixtureMapped 
  DI.ProviderFixture.Grouped
  DI.ProviderFixture.Processed.Report
  DI.FixtureManager.Conflicts
)

# Define CDC-specific topics
cdc_topics=(
  main_data.data.participantmappings
  main_data.data.leagueloadintervals
  main_data.data.participants
  main_data.data.leaguemappings
  main_data.data.locationmappings
  main_data.data.fixtures
  main_data.data.fixtureidmapping
  main_data.data.seasons
  main_data.data.locations
  main_data.data.sportintervals
  main_data.data.leagues
  main_data.data.leagueoverrides
  main_mappingdata.mappingdata.blacklistedmetadata
  main_mappingdata.mappingdata.robotconfigurations
  main_data.data.seasonmappings
  main_data.data.sports
  main_mappingdata.mappingdata.mappingconfigurations
)

# Function to create topics
create_topic() {
  local topic=$1
  local cluster=$2
  if kafka-topics.sh --bootstrap-server=localhost:9094 --list | grep -q "${topic}"; then
    echo "Topic exists - ${topic} in ${cluster} cluster"
  else
    echo "Creating topic ${topic} in ${cluster} cluster"
    kafka-topics.sh --create --topic "${topic}" --replication-factor 1 --partitions 1 --bootstrap-server localhost:9094
  fi
}

# Create hermes topics
for topic in "${hermes_topics[@]}"; do
  create_topic "$topic" "hermes"
done

# Create CDC topics
for topic in "${cdc_topics[@]}"; do
  create_topic "$topic" "CDC"
done
