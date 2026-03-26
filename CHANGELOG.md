# Changelog

All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## [Unreleased]

### Added

- **Core Functionality**: High-performance Object Pooling system for Unity, enabling efficient reuse of GameObjects to reduce instantiation overhead and improve performance.
- **Key Features**:
  - Generic and type-safe Spawn and Despawn methods for safe object management.
  - Support for dynamic pool expansion and pre-warming with configurable initial sizes.
  - Integration with custom PoolableObject interface for proper state reset and lifecycle management.
  - VContainer 1.14.0+ support with dependency injection via PoolProvider.
- **Documentation**: Initial documentation and API reference added, including usage guides, best practices, and troubleshooting.

## [1.0.0] - 2024-01-01

- Initial release
