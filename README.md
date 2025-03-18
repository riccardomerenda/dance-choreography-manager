# Dance Choreography Manager

[![Project Status](https://img.shields.io/badge/status-in%20development-yellow)](https://github.com/riccardomerenda/dance-choreography-manager)
[![License](https://img.shields.io/badge/license-MIT-blue)](LICENSE)
[![Version](https://img.shields.io/badge/version-0.1.0-brightgreen)](https://github.com/riccardomerenda/dance-choreography-manager/releases)
[![React](https://img.shields.io/badge/React-18-61DAFB?logo=react)](https://reactjs.org/)
[![TypeScript](https://img.shields.io/badge/TypeScript-4.9-3178C6?logo=typescript)](https://www.typescriptlang.org/)
[![FastAPI](https://img.shields.io/badge/FastAPI-latest-009688?logo=fastapi)](https://fastapi.tiangolo.com/)
[![PostgreSQL](https://img.shields.io/badge/PostgreSQL-latest-336791?logo=postgresql)](https://www.postgresql.org/)
[![Docker](https://img.shields.io/badge/Docker-supported-2496ED?logo=docker)](https://www.docker.com/)

## 🩰 Vision

Dance Choreography Manager is an innovative web application designed to help dance teachers manage choreographies for end-of-year performances. The system offers an intuitive interface for organizing dancers, courses, and choreographies, with a special focus on optimal dancer positioning through artificial intelligence assistance.

## 🎯 Project Goals

- **Simplify choreography management** by allowing teachers to easily visualize and modify positioning patterns
- **Optimize dancer positioning** considering gender, physique, and position history
- **Leverage artificial intelligence** to suggest optimal positions and learn from teacher preferences
- **Provide a visual drag-and-drop interface** for easy assignment of dancers to positions
- **Ensure fairness in positioning** through algorithms that track position history

## 🚀 Key Features

- 📋 Complete management of dancers, courses, and choreographies
- 🎭 Visual editor for dancer positioning
- 🧠 AI assistant for suggesting optimal positions
- 🎲 Intelligent randomized position generation
- 📊 Dashboard with statistics and analysis
- 🔄 Real-time synchronization of changes
- 🌙 Elegant and modern dark mode interface

## 💻 Technology Stack

### Frontend
- React 18 with TypeScript
- Tailwind CSS for styling
- Vite as build tool

### Backend
- FastAPI (Python)
- SQLAlchemy ORM
- Pydantic for data validation

### Database
- PostgreSQL

### ML/AI
- scikit-learn for machine learning algorithms
- NumPy and Pandas for data processing

### DevOps
- Docker and Docker Compose
- GitHub Actions for CI/CD

## 🛠️ Installation and Setup

### Prerequisites
- Docker and Docker Compose
- Node.js (v16+)
- Python 3.9+

### Local Development
1. Clone the repository:
```bash
git clone https://github.com/riccardomerenda/dance-choreography-manager.git
cd dance-choreography-manager
```

2. Start the development environment with Docker Compose:
```bash
docker-compose up -d
```

3. The frontend will be available at `http://localhost:3000`
4. The backend will be available at `http://localhost:8000`
5. API documentation will be available at `http://localhost:8000/docs`

## 📝 License

This project is distributed under the MIT license. See the `LICENSE` file for more details.

## 📈 Project Roadmap

- [x] Initial project setup and architecture
- [x] Core dancer and course management
- [ ] AI-powered positioning algorithm
- [ ] Visual choreography editor
- [ ] Performance analytics dashboard
- [ ] Mobile-responsive design
- [ ] Multi-language support

## 👥 Contributing

Contributions are welcome! Please feel free to submit a Pull Request.

1. Fork the repository
2. Create your feature branch (`git checkout -b feature/amazing-feature`)
3. Commit your changes (`git commit -m 'Add some amazing feature'`)
4. Push to the branch (`git push origin feature/amazing-feature`)
5. Open a Pull Request

## 📞 Contact

For questions or feedback, please open an issue on GitHub or contact the project maintainer.