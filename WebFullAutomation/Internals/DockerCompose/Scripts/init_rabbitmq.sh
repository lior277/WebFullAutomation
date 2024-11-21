#!/bin/bash

echo "Starting RabbitMQ initialization script..."

# Function to check RabbitMQ readiness
wait_for_rabbitmq() {
    for i in {1..10}; do
        if rabbitmqctl await_startup; then
            echo "RabbitMQ is ready."
            return 0
        else
            echo "Waiting for RabbitMQ to be ready... Attempt $i of 10"
            sleep 5
        fi
    done
    echo "RabbitMQ did not become ready in time. Exiting."
    exit 1
}

# Function to handle errors
handle_error() {
    echo "Error: $1"
    exit 1
}

# Wait for RabbitMQ to be ready
wait_for_rabbitmq || handle_error "RabbitMQ failed to start."

# Function to add a virtual host if it doesn't already exist
add_vhost_if_not_exists() {
    local vhost=$1
    if rabbitmqctl list_vhosts | grep -q "^${vhost}$"; then
        echo "Virtual host '${vhost}' already exists. Skipping creation."
    else
        if rabbitmqctl add_vhost "${vhost}"; then
            echo "Virtual host '${vhost}' created."
        else
            handle_error "Failed to create virtual host '${vhost}'."
        fi
    fi
}

# Function to set permissions for a user on a virtual host
set_permissions() {
    local vhost=$1
    if rabbitmqctl set_permissions -p "${vhost}" admin ".*" ".*" ".*"; then
        echo "Permissions set for 'admin' on virtual host '${vhost}'."
    else
        handle_error "Failed to set permissions for 'admin' on virtual host '${vhost}'."
    fi
}

# Add virtual hosts and set permissions
for vhost in InPlay Metadata PreMatch TrackChanges; do
    add_vhost_if_not_exists "${vhost}"
    set_permissions "${vhost}"
done

echo "RabbitMQ initialization script completed successfully."