/** @type {import('tailwindcss').Config} */
module.exports = {
  content: [
    "./src/**/*.{html,js,jsx,ts,tsx}"
  ],
  theme: {
    extend: {
      colors: {
        primary: "#20d249",
        "primary-200": "#74e38d",
        secondary: "#20d2a2",
        accent: "#20a9d2"
        
      }
    },
  },
  plugins: [],
}

