using ImageImporter.Data;
using ImageImporter.Models.Db;
using System.Runtime.CompilerServices;
using System.Text.Json;

namespace ImageImporter.Services
{
    public class SettingsService
    {

        private ApplicationDbContext Context { get; }

        public SettingsService(ApplicationDbContext context)
        {
            Context = context;
        }


        public void SetPar<T>(T value, [CallerMemberName] string propertyName = null)
        {
            Setting setting = Context.Settings.Where(a => a.Name == propertyName).FirstOrDefault();
            if (setting == null)
            {
                setting = new Setting();
                setting.Name = propertyName;
                Context.Settings.Add(setting);
            }

            setting.Value = JsonSerializer.Serialize(value);
            Context.SaveChanges();
        }


        public T GetPar<T>(T defVal = default(T), [CallerMemberName] string propertyName = null)
        {
            try
            {
                Setting setting = Context.Settings.Where(a => a.Name == propertyName).FirstOrDefault();
                if (setting == null)
                {
                    setting = new Setting();
                    setting.Name = propertyName;
                    setting.Value = JsonSerializer.Serialize(defVal);
                    Context.Settings.Add(setting);                      //This ensures all settings are stored in db for easy modification
                    Context.SaveChanges(true);
                    return defVal;
                }
                return JsonSerializer.Deserialize<T>(setting.Value);
            }
            catch (Exception ex)
            {

            }
            return defVal;
        }

    }



}
