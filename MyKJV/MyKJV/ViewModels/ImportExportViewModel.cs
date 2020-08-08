using Acr.UserDialogs;

using Android.Graphics;

using MyKJV.Tools;

using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MyKJV.ViewModels
{
    public class ImportExportViewModel : BaseViewModel
    {
        public ImportExportViewModel()
        {

        }
        public void Init()
        {
            Title = "Import / Export";
            IsInProgress = false;
            Position = 0;
            this.ImportPath = System.IO.Path
                .Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "Db.txt");//"/storage/9C33-6BBD/temp/db.txt";
            this.ExportPath =System.IO.Path
                .Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "Db.db3");// "/storage/9C33-6BBD/temp/";
        }
        string importPath1;
        public string ImportPath
        {
            get => this.importPath1;

            set => SetProperty<string>(ref this.importPath1, value, 
                nameof(ImportPath));
        }
        public bool UpFtp
        {
            get => upFtp;
            set => SetProperty<bool>(ref upFtp, value, nameof(UpFtp));
        }
        string exportPath1;
        public string ExportPath
        {
            get => this.exportPath1;

            set => SetProperty<string>(ref this.exportPath1, value, nameof(ExportPath));
        }
        int position;
        public int Position

        {
            get => this.position;
            set => SetProperty<int>(ref this.position, value, nameof(Position));
        }
        public bool IsInProgress { get => this.isInProgress; set => SetProperty<bool>(ref this.isInProgress, value, nameof(IsInProgress)); }

        private bool isInProgress;
        private bool upFtp;

        public async Task<bool> ImportDbFromCsv(Action<double, uint> action)
        {
            IsInProgress = true;
            try
            {
                await Task.Factory.StartNew(() => StarterData.ImportDBOrigCSV(action));

                return true;
            }
            catch (Exception ex)
            {
                UserDialogs.Instance.Alert($"An error occurred while attempting to import:\n{ex}");
                return false;
            }
            finally
            {
                IsInProgress = false;
            }
        }
        public async Task<bool> ImportDb(Action<double, uint> action)
        {
            IsInProgress = true;
            try
            {
                await Task.Factory.StartNew(() => StarterData.ImportDb(ImportPath, action));

                return true;
            }
            catch (Exception ex)
            {
                UserDialogs.Instance.Alert($"An error occurred while attempting to import:\n{ex}");
                return false;
            }
            finally
            {
                IsInProgress = false;
            }
        }
        public async Task<bool> ExportDb(Action<double, uint> action )
        {
            IsInProgress = true;
            try
            {
                await Task.Factory.StartNew(() => StarterData.ExportDb(ExportPath, action, UpFtp));

                return true;
            }
            catch (Exception ex)
            {
                UserDialogs.Instance.Alert($"An error occurred while attempting to export:\n{ex}");
                return false;
            }
            finally
            {
                IsInProgress = false;
            }
        }
        public async Task<bool> ClearDb()
        {
            IsInProgress = true;
            try
            {
                await Task.Factory.StartNew(() => StarterData.ClearDb(null));
                return true;
            }
            catch (Exception ex)
            {
                UserDialogs.Instance.Alert($"An error occurred while attempting to clear Db:\n{ex}");
                return false;
            }

            finally
            {
                IsInProgress = false;

            }
        }
    }
}
