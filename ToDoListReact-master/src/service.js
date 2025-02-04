import axios from 'axios';

const apiUrl = "http://localhost:5000"

export default {
  
  getTasks: async () => {
    const result1 = await axios.get(`${apiUrl}/api/tasks`)    
    return result1.data;
  },

  addTask: async(name)=>{  
    const currentSecond= new Date().getSeconds();
    console.log('addTask', name)
    const result= await axios.post(`${apiUrl}/api/tasks`,
      {Id:currentSecond,Name:name,IsComplete:false})
      return result.data;
    // return {};
  },

  setCompleted: async(id, isComplete)=>{
    console.log('setCompleted', {id, isComplete})
    const result = await axios.put(`${apiUrl}/api/tasks/${id}`,{id,isComplete})    
    return result.data;
  },

  deleteTask:async(id)=>{
    console.log('deleteTask')
    const result= await axios.delete(`${apiUrl}/api/tasks/${id}`,id)
    return result.data;
  }
}
