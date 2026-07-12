const { env } = require('process');

const target = env.ASPNETCORE_HTTPS_PORT ? `https://localhost:${env.ASPNETCORE_HTTPS_PORT}` :
  env.ASPNETCORE_URLS ? env.ASPNETCORE_URLS.split(';')[0] : 'https://localhost:7026';

const PROXY_CONFIG = [
  {
    context: [
      "/api/listquestions/questions",
      "/api/syllabus/syllabusname",
      "/api/exam/submit",
      "/api/ExamSubmit/examsubmits",
      "/api/ticket",
      "/api/empuser",
      "/api/performance-report",
      "/api/Tests",
      "/api/Login/login",
      "/api/Login/registration",
      "/api/Login/emailvarifcation",
      "/api/UserProfile/updateuserprofile",
      "/api/UserProfile/changepassword",
      "/api/UserProfile/getmeprofile"
    ],
    target,
    secure: false
  }
]

module.exports = PROXY_CONFIG;
