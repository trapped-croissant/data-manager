# Use a Node.js base image
FROM node:lts

# Set the working directory in the container
WORKDIR /app

# Copy package.json and package-lock.json
COPY package*.json ./

# Install dependencies
RUN npm install

# Copy the rest of the application code
COPY . .

# Build the React application
#RUN npm run build

# Expose the port the app will run on
EXPOSE 4001

CMD ["npm", "start"]