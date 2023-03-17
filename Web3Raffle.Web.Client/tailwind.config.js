module.exports = {
  content: [
    "**/*.razor",
    "**/*.cshtml",
    "**/*.cs",
    "**/*.html"],
  theme: {
    fontFamily: {
      'sans': ['Inter', 'Helvetica', 'Arial', 'sans-serif'],
      'serif': ['Inter', 'Helvetica', 'Arial', 'sans-serif'],
      'mono': ['Inter', 'Helvetica', 'Arial', 'sans-serif']
    },
    extend: {
      backgroundColor: {
        hero: {
          '100': 'background-color: rgb(4 65 97)'
        }
      },
      maxWidth: {
        'max-w-8xl': '96rem',
        'max-w-9xl': '108rem'
      }
    },
    screens: {
      'mobile': '475px',
      'sm': '640px',
      'md': '768px',
      'lg': '1024px',
      'xl': '1280px',
      '2xl': '1536px',
      '3xl': '1728px'
    }
  },
  plugins: [
    require('@tailwindcss/typography'),
    require('@tailwindcss/forms')
  ],
}