import axios from 'axios';
import config from './config';

//const apiUrl = "http://localhost:5000"

export default {  
  getTasks: async () => {
    const result1 = await axios.get(`${config.apiUrl}/api/tasks`)    
    return result1.data;
  },
  addTask: async(name)=>{  
    const currentSecond= new Date().getSeconds();
    console.log('addTask', name)
    const result= await axios.post(`${config.apiUrl}/api/tasks`,
      {Id:currentSecond,Name:name,IsComplete:false})
      return result.data;
  },

  setCompleted: async(id, isComplete)=>{
    console.log('setCompleted', {id, isComplete})
    const result = await axios.put(`${config.apiUrl}/api/tasks/${id}`,{IsComplete: isComplete})    
    return result.data;
  },

  deleteTask:async(id)=>{
    console.log('deleteTask')
    const result= await axios.delete(`${config.apiUrl}/api/tasks/${id}`,id)
    return result.data;
  }
}
