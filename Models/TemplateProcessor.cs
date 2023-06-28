using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;

namespace Documently.Models;

/*
 * Интерфейс для взаимодействия с процессором шаблонов
 * Реализуйте его в своём бэкенде
 */
public interface ITemplateProcessor
{
    /*
     * Настроить окружение процессора
     * name    - путь к файлу шаблона, который заполняется
     * path    - путь к папке, куда необходимо записывать готовые документы
     * pattern - шаблон имени результирующего файла
     */
    public Aspose.Words.Document Setup(MemoryStream name, string path, string pattern);

    //public void Setup(string name, string path, string pattern);
    /*
     * Извлечь список полей из шаблона
     */
    public Dictionary<string, ObservableCollection<Field>> GetFields();

    /*
     * Заполнить шаблон
     * record - набор значений для одного экземпляра
     */
    public Aspose.Words.Document Fill(Dictionary<string, ObservableCollection<Field>> record);

    /*
     * Освободить ресурсы, занятые процессором
     * Сюда входит какая-либо память, файловые потоки и прочее
     */
    public void Dispose();

    public void Save(Aspose.Words.Document doc, string extension);
}